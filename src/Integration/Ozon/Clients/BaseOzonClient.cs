using System.Net;
using System.Text;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Ozon;

namespace Integration.Ozon.Clients;

public abstract class BaseOzonClient
{
    protected BaseOzonClient()
    {
    }

    protected static async Task<HttpResponseMessage> PostAsync(OzonDto ozon, string url, object obj)
    {
        var client = new HttpClient();

        string fullUrl =
            $"https://{ozon.Settings.Server}:{ozon.Settings.Port}/{url}";

        var httpMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl);

        httpMessage.Content = new StringContent(obj.ToJson(), Encoding.UTF8, "application/json");
        httpMessage.Headers.Add("Client-Id", ozon.Settings.ClientId);
        httpMessage.Headers.Add("Api-Key", ozon.Settings.ApiKey);
        httpMessage.Headers.Add("cache-disable", Guid.NewGuid().ToString());

        HttpResponseMessage httpResponse = await client.SendAsync(httpMessage);

        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            string body = await httpResponse.Content.ReadAsStringAsync();
            string message = $"Send Ozon request failure." +
                             $"{Environment.NewLine}Url: {fullUrl}" +
                             $"{Environment.NewLine}Status code: {httpResponse.StatusCode}" +
                             $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }

        return httpResponse;
    }
}