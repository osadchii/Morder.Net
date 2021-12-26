using Infrastructure.Services.Marketplaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IChangeTrackingService, ChangeTrackingService>();
    }
}