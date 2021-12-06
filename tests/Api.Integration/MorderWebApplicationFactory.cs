using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Integration;

public class MorderWebApplicationFactory : WebApplicationFactory<Program>
{
    public static string ServiceUrl(int apiVersion, string path)
    {
        return $"api/v{apiVersion}/{path}";
    }
}