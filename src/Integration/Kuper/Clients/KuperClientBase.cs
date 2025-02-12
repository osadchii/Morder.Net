using System.Net;
using System.Net.Http.Headers;
using Infrastructure.Extensions;
using Infrastructure.Models.Marketplaces.Kuper;
using Newtonsoft.Json;

namespace Integration.Kuper.Clients;

public abstract class KuperClientBase
{
    public async Task<string> GetToken(KuperDto kuper, HttpClient httpClient)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, kuper.Settings.AuthUrl);

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", kuper.Settings.ClientId },
            { "client_secret", kuper.Settings.ClientSecret },
            { "grant_type", "client_credentials" }
        });

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error while getting token from Kuper auth url: " + kuper.Settings.AuthUrl +
                                ". StatusCode: " + response.StatusCode + " Error:" +
                                await response.Content.ReadAsStringAsync());
        }

        var body = await response.Content.ReadAsObject<KuperTokenModel>();

        return body.AccessToken;
    }

    protected async Task<HttpResponseMessage> PostAsync(KuperDto kuper, string url, object obj, string token = null)
    {
        using var client = new HttpClient();

        if (token.IsNullOrEmpty())
        {
            token = await GetToken(kuper, client);
        }

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var fullUrl =
            $"{kuper.Settings.ApiUrl}{url}";

        var httpMessage = new HttpRequestMessage(HttpMethod.Post, fullUrl);

        var jsonObj = obj.ToJson();

        httpMessage.Content = new StringContent(jsonObj, null, "application/json");
        httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var httpResponse = await client.SendAsync(httpMessage);

        if (httpResponse.StatusCode == HttpStatusCode.OK)
        {
            return httpResponse;
        }

        var body = await httpResponse.Content.ReadAsStringAsync();
        var message = $"Send Kuper request failure." +
                      $"{Environment.NewLine}Url: {fullUrl}" +
                      $"{Environment.NewLine}Content-Type: {httpMessage.Content.Headers.ContentType}" +
                      $"{Environment.NewLine}Body: {jsonObj}" +
                      $"{Environment.NewLine}Status code: {httpResponse.StatusCode}" +
                      $"{Environment.NewLine}Message: {body}";

        throw new Exception(message);
    }

    protected async Task<HttpResponseMessage> PutAsync(KuperDto kuper, string url, object obj, string token = null)
    {
        using var client = new HttpClient();

        if (token.IsNullOrEmpty())
        {
            token = await GetToken(kuper, client);
        }

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var fullUrl =
            $"{kuper.Settings.ApiUrl}{url}";

        var httpMessage = new HttpRequestMessage(HttpMethod.Put, fullUrl);

        var jsonObj = obj.ToJson();

        httpMessage.Content = new StringContent(jsonObj, null, "application/json");
        httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var httpResponse = await client.SendAsync(httpMessage);

        if (httpResponse.StatusCode == HttpStatusCode.OK)
        {
            return httpResponse;
        }

        var body = await httpResponse.Content.ReadAsStringAsync();
        var message = $"Send Kuper request failure." +
                      $"{Environment.NewLine}Url: {fullUrl}" +
                      $"{Environment.NewLine}Content-Type: {httpMessage.Content.Headers.ContentType}" +
                      $"{Environment.NewLine}Body: {jsonObj}" +
                      $"{Environment.NewLine}Status code: {httpResponse.StatusCode}" +
                      $"{Environment.NewLine}Message: {body}";

        throw new Exception(message);
    }

    protected async Task<HttpResponseMessage> GetAsync(KuperDto kuper, string url, string token = null)
    {
        using var client = new HttpClient();

        if (token.IsNullOrEmpty())
        {
            token = await GetToken(kuper, client);
        }

        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

        var fullUrl =
            $"{kuper.Settings.ApiUrl}{url}";

        var httpMessage = new HttpRequestMessage(HttpMethod.Get, fullUrl);

        var httpResponse = await client.SendAsync(httpMessage);

        if (httpResponse.StatusCode == HttpStatusCode.OK)
        {
            return httpResponse;
        }

        var body = await httpResponse.Content.ReadAsStringAsync();
        var message = $"Send Kuper request failure." +
                      $"{Environment.NewLine}Url: {fullUrl}" +
                      $"{Environment.NewLine}Status code: {httpResponse.StatusCode}" +
                      $"{Environment.NewLine}Message: {body}";

        throw new Exception(message);
    }

    private class KuperTokenModel
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; } = null!;

        [JsonProperty("expires_in")] public long ExpiresIn { get; set; }
    }
}