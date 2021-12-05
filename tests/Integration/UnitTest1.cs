using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using var app = new WebApplicationFactory<Program>();
        using var client=app.CreateClient();
    }
}