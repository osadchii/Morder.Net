using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonRejectOrderClient
{
    Task<bool> RejectOrder(OzonDto ozonDto, RejectPostingRequest request);
}

public class OzonRejectOrderClient : BaseOzonClient, IOzonRejectOrderClient
{
    public async Task<bool> RejectOrder(OzonDto ozonDto, RejectPostingRequest request)
    {
        var httpResponse = await PostAsync(ozonDto, "v2/posting/fbs/cancel", request);
        var body = await httpResponse.Content.ReadAsStringAsync();

        var response = body.FromJson<RejectPostingResponse>();

        if (response is null)
        {
            var message = $"Ozon reject order error." +
                          $"{Environment.NewLine}Can't deserialize body: {body}";

            throw new Exception(message);
        }

        return response.Result;
    }
}