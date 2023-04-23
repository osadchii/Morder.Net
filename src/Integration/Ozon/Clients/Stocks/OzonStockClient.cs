using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Stocks.Messages;

namespace Integration.Ozon.Clients.Stocks;

public interface IOzonStockClient
{
    Task SendStocks(OzonDto ozon, OzonStockRequest request);
}

public class OzonStockClient : BaseOzonClient, IOzonStockClient
{
    public async Task SendStocks(OzonDto ozon, OzonStockRequest request)
    {
        await PostAsync(ozon, "v2/products/stocks", request);
    }
    
    [Obsolete("Old implementation for 1 warehouse case")]
    public async Task SendStocks_Old(OzonDto ozon, OzonStockRequest request)
    {
        await PostAsync(ozon, "v1/product/import/stocks", request);
    }
}