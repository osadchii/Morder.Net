namespace Infrastructure.Models.Marketplaces.Kuper;

public class KuperSettings
{
    public string AuthUrl { get; set; } = null!;
    public string ApiUrl { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public bool? SendingEnabled { get; set; }
    public bool? LoadOrders { get; set; }
    public bool? LoadOrdersAsArchived { get; set; }
}