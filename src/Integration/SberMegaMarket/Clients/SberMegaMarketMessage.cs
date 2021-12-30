using Newtonsoft.Json;

namespace Integration.SberMegaMarket.Clients;

public class SberMegaMarketMessage<T> where T : SberMegaMarketMessageData, new()
{
    public SberMegaMarketMessage(string token)
    {
        Meta = new MessageMeta();
        Data = new T
        {
            Token = token
        };
    }

    [JsonProperty("meta")] public MessageMeta Meta { get; set; }

    [JsonProperty("data")] public T Data { get; set; }

    public class MessageMeta
    {
    }
}

public abstract class SberMegaMarketMessageData
{
    [JsonProperty("token")] public string Token { get; set; }

    protected SberMegaMarketMessageData(string token)
    {
        Token = token;
    }
}