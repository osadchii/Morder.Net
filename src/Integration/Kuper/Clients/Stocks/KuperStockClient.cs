using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Kuper.Clients.Stocks.Messages;

namespace Integration.Kuper.Clients.Stocks;

public interface IKuperStockClient
{
    Task SendStocks(KuperDto kuper, KuperStockMessage message);
}

public class KuperStockClient : KuperClientBase, IKuperStockClient
{
    public Task SendStocks(KuperDto kuper, KuperStockMessage message)
    {
        return PostAsync(kuper, "/v1/import/stocks", message);
    }
}