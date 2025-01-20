using Infrastructure.Services.Marketplaces;
using Infrastructure.Services.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IChangeTrackingService, ChangeTrackingService>();
        services.AddTransient<IJwtTokenService, JwtTokenService>();
        services.AddTransient<IProductIdentifierService, ProductIdentifierService>();
        services.AddTransient<IProductArticulService, ProductArticulService>();
        services.AddTransient<IProductImageService, ProductImageService>();
    }
}