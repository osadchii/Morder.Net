namespace Infrastructure.Models.Prices;

public class PriceTypeDto
{
    public string Name { get; set; }

    public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}