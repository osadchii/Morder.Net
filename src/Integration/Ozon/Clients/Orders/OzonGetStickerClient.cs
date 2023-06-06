using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Ozon.Clients.Orders.Messages;

namespace Integration.Ozon.Clients.Orders;

public interface IOzonGetStickerClient
{
    Task<byte[]> GetSticker(OzonDto ozon, GetStickerRequest request);
}

public class OzonGetStickerClient : BaseOzonClient, IOzonGetStickerClient
{
    public async Task<byte[]> GetSticker(OzonDto ozon, GetStickerRequest request)
    {
        HttpResponseMessage response = await PostAsync(ozon, "v2/posting/fbs/package-label", request);
        var body = await response.Content.ReadAsByteArrayAsync();

        if (body is null)
        {
            throw new Exception($"Can't deserialize body: {body}");
        }

        return body;
    }
}