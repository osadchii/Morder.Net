using Infrastructure.Database;
using Infrastructure.MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddMorder(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<MContext>(options => { options.UseNpgsql(connectionString); });

        services.AddMorderAutoMapper();
        services.AddMorderMediatR();

        services.AddTransient<IMigrationService, MigrationService>();
    }
}