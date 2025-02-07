using Integration.Kuper.Clients.Offers;
using Integration.Kuper.Clients.Orders;
using Integration.Kuper.Clients.Prices;
using Integration.Kuper.Clients.Stocks;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Kuper;

public static class DependencyInjection
{
    public static void AddKuper(this IServiceCollection services)
    {
        services.AddTransient<IKuperOrdersClient, KuperOrdersClient>();
        services.AddTransient<IKuperPriceClient, KuperPriceClient>();
        services.AddTransient<IKuperStockClient, KuperStockClient>();
        services.AddTransient<IKuperOfferClient, KuperOfferClient>();
        services.AddTransient<IKuperOrderAdapter, KuperOrderAdapter>();
    }
}