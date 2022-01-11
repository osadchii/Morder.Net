using Integration.Meso.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Meso;

public static class DependencyInjection
{
    public static void AddMeso(this IServiceCollection services)
    {
        services.AddTransient<IMesoSendFeedClient, MesoSendFeedClient>();
    }
}