using System.Globalization;
using Api.BackgroundServices;
using Api.Bot;
using Api.Extensions;
using Api.Filters;
using Infrastructure;
using Infrastructure.Bot;
using Infrastructure.Constants;
using Integration;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
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
        var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Configurations");

        builder.Configuration
            .AddJsonFile(Path.Combine(sharedFolder, "appsettings.json"), true)
            .AddJsonFile(Path.Combine(sharedFolder, $"appsettings.{env.EnvironmentName}.json"), true)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ServiceExceptionFilter>();
                options.Filters.Add(new AuthorizeFilter());
            })
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

        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddLogging(logBuilder =>
        {
            logBuilder.AddConfiguration(builder.Configuration);
#if DEBUG
            logBuilder.AddConsole();
#else
            logBuilder.AddJsonConsole();
#endif
            logBuilder.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
            logBuilder.AddFilter("System.Net.Http.HttpClient.tgwebhook.ClientHandler", LogLevel.Warning);
            logBuilder.AddFilter("System.Net.Http.HttpClient.tgwebhook.LogicalHandler", LogLevel.Warning);
            logBuilder.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Information);
// #if !DEBUG
            logBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
// #endif
        });

        builder.Services.AddHttpLogging(_ => {});

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddMorder(builder.Configuration);

        if (env.EnvironmentName != EnvironmentNames.TestEnvironment)
        {
            builder.Services.AddMorderBot(builder.Configuration);
            builder.Services.AddBackgroundServices();
        }
        
        builder.Services.AddMarketplaces();
        builder.Services.AddMemoryCache();

        WebApplication app = builder.Build();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseHsts();

        app.UseHttpLogging();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();	
        app.UseAuthorization();
        app.UseStaticFiles();

        app.MapControllers();

        if (env.EnvironmentName != EnvironmentNames.TestEnvironment)
        {
            InitializeDatabase(app);
        }

        var config = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

        app.UseEndpoints(endpoints =>
        {
            var token = config.BotToken!;
            endpoints.MapControllerRoute(name: "tgwebhook",
                pattern: $"bot/{token}",
                new { controller = "Telegram", action = "Post" });
            endpoints.MapControllers();
        });

        app.Run();
    }

    private static void InitializeDatabase(IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        scope?.ServiceProvider.GetRequiredService<MContext>().Database.Migrate();
    }
}