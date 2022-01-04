using System.Globalization;
using Api.BackgroundServices.Marketplaces;
using Api.BackgroundServices.Marketplaces.SberMegaMarketServices;
using Api.Filters;
using Infrastructure;
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
        builder.Services.AddMarketplaces();
        builder.Services.AddMemoryCache();
        builder.Services.AddHostedService<SberMegaMarketFeedService>();
        builder.Services.AddHostedService<SendStockBackgroundService>();
        builder.Services.AddHostedService<SendPriceBackgroundService>();
        builder.Services.AddHostedService<LoadProductIdsBackgroundService>();
        builder.Services.AddHostedService<MarketplaceOrderTaskExecutorService>();
        builder.Services.AddHostedService<LoadOrdersBackgroundService>();

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

        app.Run();
    }

    private static void InitializeDatabase(IApplicationBuilder app)
    {
        using IServiceScope? scope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        scope?.ServiceProvider.GetRequiredService<MContext>().Database.Migrate();
    }
}