using Infrastructure.Cache;
using Infrastructure.Mappings;
using Infrastructure.MediatR;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddMorder(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");
        services.AddDbContext<MContext>((_, options) =>
        {
            options.UseNpgsql(connectionString,
                builder => { builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
        });

        services.AddMorderAutoMapper();
        services.AddMorderMediatR();
        services.AddCacheServices();
        services.AddServices();
    }
}