using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonPackOrderClient
{
    Task<PackPostingResponse> PackOrder(OzonDto ozon, PackPostingRequest request);
}

public class OzonPackOrderClient : BaseOzonClient, IOzonPackOrderClient
{
    public async Task<PackPostingResponse> PackOrder(OzonDto ozon, PackPostingRequest request)
    {
        var httpResponse = await PostAsync(ozon, "v4/posting/fbs/ship", request);
        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<PackPostingResponse>();

        if (response is null)
        {
            var message = $"Ozon pack order error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        return response;
    }
}