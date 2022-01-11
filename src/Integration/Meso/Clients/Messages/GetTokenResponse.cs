using Newtonsoft.Json;

namespace Integration.Meso.Clients.Messages;

public record GetTokenResponse
{
    [JsonProperty("token")] public string Token { get; set; }
}