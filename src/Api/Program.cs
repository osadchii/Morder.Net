using System.Globalization;
using Api.BackgroundServices;
using Api.Filters;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            { Converters = { new StringEnumConverter { CamelCaseText = true } } };

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
            logBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMorder(builder.Configuration);
        builder.Services.AddMemoryCache();
        builder.Services.AddHostedService<SberMegaMarketFeedService>();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

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