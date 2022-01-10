namespace Infrastructure.Models.Marketplaces.Meso;

public class MesoSettings
{
    public string Server { get; set; } = "";
    public int Port { get; set; } = 443;
    public string Login { get; set; }
    public string Password { get; set; }
    public bool SaveFeed { get; set; }
    public string FeedName { get; set; }
}