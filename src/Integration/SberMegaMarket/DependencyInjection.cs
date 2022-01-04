using Integration.SberMegaMarket.Clients;
using Integration.SberMegaMarket.Clients.Interfaces;
using Integration.SberMegaMarket.Clients.Orders.Messages;
using Integration.SberMegaMarket.Clients.Prices.Messages;
using Integration.SberMegaMarket.Clients.Stocks.Messages;
using Integration.SberMegaMarket.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.SberMegaMarket;

public static class DependencyInjection
{
    public static void AddSberMegaMarket(this IServiceCollection services)
    {
        services.AddTransient<ISberMegaMarketClient<SendStockData>, SberMegaMarketClient<SendStockData>>();
        services.AddTransient<ISberMegaMarketClient<SendPriceData>, SberMegaMarketClient<SendPriceData>>();
        services.AddTransient<ISberMegaMarketClient<OrderConfirmData>, SberMegaMarketClient<OrderConfirmData>>();
        services.AddTransient<ISberMegaMarketClient<OrderPackingData>, SberMegaMarketClient<OrderPackingData>>();
        services.AddTransient<ISberMegaMarketClient<OrderShippingData>, SberMegaMarketClient<OrderShippingData>>();
        services.AddTransient<ISberMegaMarketClient<OrderRejectingData>, SberMegaMarketClient<OrderRejectingData>>();
        services.AddTransient<ISberMegaMarketClient<StickerPrintData>, SberMegaMarketClient<StickerPrintData>>();
        services.AddTransient<ISberMegaMarketClient<LoadOrdersData>, SberMegaMarketClient<LoadOrdersData>>();

        services.AddTransient<ISberMegaMarketOrderAdapter, SberMegaMarketOrderAdapter>();
    }
}