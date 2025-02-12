using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Kuper.Clients.Prices.Messages;

namespace Integration.Kuper.Clients.Prices;

public interface IKuperPriceClient
{
    Task SendPrices(KuperDto kuper, KuperPriceMessage message);
}

public class KuperPriceClient : KuperClientBase, IKuperPriceClient
{
    public Task SendPrices(KuperDto kuper, KuperPriceMessage message)
    {
        return PutAsync(kuper, "/v2/import/prices", message);
    }
}