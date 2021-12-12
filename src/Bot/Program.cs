using System.Globalization;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bot;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

        IWebHostEnvironment? env = builder.Environment;
        string sharedFolder = Path.Combine(env.ContentRootPath, "..", "Configurations");

        builder.Configuration
            .AddJsonFile(Path.Combine(sharedFolder, "appsettings.json"), true)
            .AddJsonFile(Path.Combine(sharedFolder, $"appsettings.{env.EnvironmentName}.json"), true)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
                {
                    DateTimeStyles = DateTimeStyles.AdjustToUniversal
                });
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

        builder.Services.AddLogging(logBuilder =>
        {
            logBuilder.AddConfiguration(builder.Configuration);
            logBuilder.AddConsole();
        });

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddMorder(builder.Configuration);
        builder.Services.AddMorderBot(builder.Configuration);
        builder.Services.AddMemoryCache();

        WebApplication? app = builder.Build();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseRouting();
        app.UseHttpsRedirection();

        var config = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

        app.UseEndpoints(endpoints =>
        {
            string token = config.BotToken;
            endpoints.MapControllerRoute(name: "tgwebhook",
                pattern: $"bot/{token}",
                new { controller = "Webhook", action = "Post" });
            endpoints.MapControllers();
        });

        app.Run();
    }
}