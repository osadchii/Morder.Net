using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.Models.Companies;
using Xunit;

namespace Api.Integration.Companies;

public class CompanyTests : IClassFixture<MorderWebApplicationFactory>
{
    private readonly MorderWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CompanyTests(MorderWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UpdateCompanyInformationFail()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, "company");

        var request = new UpdateCompanyInformationRequest();
        HttpResponseMessage response = await _client.PostAsync(url, JsonContent.Create(request));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCompanyInformationSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, "company");
        HttpResponseMessage response =
            await _client.PostAsync(url, JsonContent.Create(TestCases.UpdateCompanyInformationRequest));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCompanyInformationSuccess()
    {
        string url = MorderWebApplicationFactory.ServiceUrl(1, "company");
        HttpResponseMessage response = await _client.GetAsync(url);

        string content = await response.Content.ReadAsStringAsync();
        var result = content.FromJson<Result<CompanyDto>>();

        Assert.NotNull(result);
        Assert.Equal(TestCases.UpdateCompanyInformationRequest.Name, result?.Value.Name);
        Assert.Equal(TestCases.UpdateCompanyInformationRequest.Shop, result?.Value.Shop);
        Assert.Equal(TestCases.UpdateCompanyInformationRequest.Url, result?.Value.Url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}