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
        HttpResponseMessage httpResponse = await PostAsync(ozon, "v3/posting/fbs/ship", request);
        string body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<PackPostingResponse>();

        if (response is null)
        {
            string message = $"Ozon pack order error." +
                             $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        return response;
    }
}