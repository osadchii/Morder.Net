using Newtonsoft.Json;

namespace Integration.Ozon.Clients.Orders.Messages;

public class GetStickerRequest
{
    [JsonProperty("posting_number")] public IEnumerable<string> PostingNumber { get; set; } = null!;

    public GetStickerRequest()
    {
    }

    public GetStickerRequest(string number)
    {
        PostingNumber = new[]
        {
            number
        };
    }
}