using Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestFramework;

public abstract class BaseTest : IClassFixture<MorderWebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;

    protected BaseTest(MorderWebApplicationFactory<Program> factory)
    {
        Client = factory.CreateClient();
    }
}