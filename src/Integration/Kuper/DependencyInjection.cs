using Integration.Kuper.Clients.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Kuper;

public static class DependencyInjection
{
    public static void AddKuper(this IServiceCollection services)
    {
        services.AddTransient<IKuperOrdersClient, KuperOrdersClient>();
    }
}