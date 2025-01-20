using System.Net;
using System.Text;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Meso;
using Integration.Meso.Clients.Messages;
using Integration.Meso.Feeds;

namespace Integration.Meso.Clients;

public interface IMesoSendFeedClient
{
    Task SendFeed(MesoDto meso, Feed feed);
}

public class MesoSendFeedClient : IMesoSendFeedClient
{
    private readonly HttpClient _httpClient;

    public MesoSendFeedClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task SendFeed(MesoDto meso, Feed feed)
    {
        var token = await GetToken(meso);
        await PostAsync(meso, "api/store/integration/catalog/upload", feed, token);
    }

    private async Task<string> GetToken(MesoDto meso)
    {
        var request = new GetTokenRequest()
        {
            Username = meso.Settings.Login,
            Password = meso.Settings.Password
        };

        var response = await PostAsync(meso, "api/store/token/request", request);
        var content = await response.Content.ReadAsStringAsync();

        var mesoResponse = content.FromJson<GetTokenResponse>();

        if (mesoResponse is null)
        {
            throw new Exception("Can't deserialize Meso GetTokenResponse." +
                                $"{Environment.NewLine}Content: {content}");
        }

        return mesoResponse.Token;
    }

    private async Task<HttpResponseMessage> PostAsync(MesoDto meso, string url, object obj, string token = null)
    {
        var fullUrl =
            $"https://{meso.Settings.Server}:{meso.Settings.Port}/{url}";

        var httpMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl);

        httpMessage.Content = new StringContent(obj.ToJson(), Encoding.UTF8, "application/json");

        if (!token.IsNullOrEmpty())
        {
            httpMessage.Headers.Add("Authorization", $"Bearer {token}");
        }

        var httpResponse = await _httpClient.SendAsync(httpMessage);

        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            var body = await httpResponse.Content.ReadAsStringAsync();
            var message = $"Send Meso request failure." +
                          $"{Environment.NewLine}Url: ${fullUrl}" +
                          $"{Environment.NewLine}Status code: ${httpResponse.StatusCode}" +
                          $"{Environment.NewLine}Message: {body}";

            throw new Exception(message);
        }

        return httpResponse;
    }
}