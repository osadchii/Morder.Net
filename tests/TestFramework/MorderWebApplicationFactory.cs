using Infrastructure;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestFramework;

public class MorderWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(EnvironmentNames.TestEnvironment);
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MContext>))!;
            services.Remove(descriptor);

            services.AddDbContext<MContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            ServiceProvider sp = services.BuildServiceProvider();

            using IServiceScope scope = sp.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<MContext>();

            db.Database.EnsureCreated();
        });
    }
}