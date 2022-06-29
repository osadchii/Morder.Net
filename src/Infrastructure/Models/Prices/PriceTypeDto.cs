namespace Infrastructure.Models.Prices;

public class PriceTypeDto
{
    public string Name { get; set; } = null!;

    public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}