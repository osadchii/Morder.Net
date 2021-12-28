namespace Infrastructure.Models.Marketplaces.Ozon;

public class OzonSettings
{
    public string ClientId { get; set; }
    public string ApiKey { get; set; }
    public string Server { get; set; } = "";
    public int Port { get; set; } = 443;
    public bool LoadProductIds { get; set; } = false;
    public int LoadProductIdsPageSize { get; set; } = 100;
}