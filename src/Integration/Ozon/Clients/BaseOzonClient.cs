using System.Net;
using System.Text;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;

namespace Integration.Ozon.Clients;

public abstract class BaseOzonClient
{
    protected static async Task<HttpResponseMessage> PostAsync(OzonDto ozon, string url, object obj)
    {
        var client = new HttpClient();

        var fullUrl =
            $"https://{ozon.Settings.Server}:{ozon.Settings.Port}/{url}";

        var httpMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl);

        var jsonObj = obj.ToJson();

        httpMessage.Content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
        httpMessage.Headers.Add("Client-Id", ozon.Settings.ClientId);
        httpMessage.Headers.Add("Api-Key", ozon.Settings.ApiKey);
        httpMessage.Headers.Add("cache-disable", Guid.NewGuid().ToString());

        var httpResponse = await client.SendAsync(httpMessage);

        if (httpResponse.StatusCode == HttpStatusCode.OK)
        {
            return httpResponse;
        }

        var body = await httpResponse.Content.ReadAsStringAsync();
        var message = $"Send Ozon request failure." +
                      $"{Environment.NewLine}Url: {fullUrl}" +
                      $"{Environment.NewLine}Body: {jsonObj}" +
                      $"{Environment.NewLine}Status code: {httpResponse.StatusCode}" +
                      $"{Environment.NewLine}Message: {body}";

        throw new Exception(message);
    }
}