using Newtonsoft.Json;

namespace Integration.Meso.Clients.Messages;

public record GetTokenRequest
{
    [JsonProperty("username")] public string Username { get; set; } = null!;

    [JsonProperty("password")] public string Password { get; set; } = null!;
}