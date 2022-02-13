using System.Globalization;
using Api.BackgroundServices;
using Api.Bot;
using Api.Filters;
using Infrastructure;
using Infrastructure.Bot;
using Integration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Converters =
            {
                new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                    AllowIntegerValues = false
                }
            }
        };

        IWebHostEnvironment env = builder.Environment;
        string sharedFolder = Path.Combine(env.ContentRootPath, "..", "Configurations");

        builder.Configuration
            .AddJsonFile(Path.Combine(sharedFolder, "appsettings.json"), true)
            .AddJsonFile(Path.Combine(sharedFolder, $"appsettings.{env.EnvironmentName}.json"), true)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers(options => { options.Filters.Add<ServiceExceptionFilter>(); })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
                {
                    DateTimeStyles = DateTimeStyles.AdjustToUniversal
                });
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        builder.Services.AddLogging(logBuilder =>
        {
            logBuilder.AddConfiguration(builder.Configuration);
            logBuilder.AddConsole();
            logBuilder.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
#if !DEBUG
            logBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
#endif
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddMorder(builder.Configuration);
        builder.Services.AddMorderBot(builder.Configuration);
        builder.Services.AddMarketplaces();
        builder.Services.AddMemoryCache();
        builder.Services.AddBackgroundServices();

        WebApplication app = builder.Build();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseHsts();

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapControllers();

        InitializeDatabase(app);

        var config = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

        app.UseEndpoints(endpoints =>
        {
            string token = config.BotToken!;
            endpoints.MapControllerRoute(name: "tgwebhook",
                pattern: $"bot/{token}",
                new { controller = "Telegram", action = "Post" });
            endpoints.MapControllers();
        });

        app.Run();
    }

    private static void InitializeDatabase(IApplicationBuilder app)
    {
        using IServiceScope? scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        scope?.ServiceProvider.GetRequiredService<MContext>().Database.Migrate();
    }
}