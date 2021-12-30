using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure.Models.Marketplaces.Ozon;

namespace Integration.Ozon.Clients;

public abstract class BaseOzonClient
{
    private readonly HttpClient _client;

    protected BaseOzonClient()
    {
        _client = new HttpClient();
    }

    protected async Task<HttpResponseMessage> PostAsync(OzonDto ozon, string url, object obj)
    {
        string fullUrl =
            $"https://{ozon.Settings.Server}:{ozon.Settings.Port}/{url}";

        var httpMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl);
        var jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        httpMessage.Content = JsonContent.Create(obj, options: jsonOptions);
        httpMessage.Headers.Add("Client-Id", ozon.Settings.ClientId);
        httpMessage.Headers.Add("Api-Key", ozon.Settings.ApiKey);

        HttpResponseMessage httpResponse = await _client.SendAsync(httpMessage);

        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            string body = await httpResponse.Content.ReadAsStringAsync();
            string message = $"Send Ozon request failure." +
                             $"{Environment.NewLine}Url: ${fullUrl}" +
                             $"{Environment.NewLine}Status code: ${httpResponse.StatusCode}" +
                             $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }

        return httpResponse;
    }
}