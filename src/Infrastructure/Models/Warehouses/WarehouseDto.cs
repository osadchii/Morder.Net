namespace Infrastructure.Models.Warehouses;

public class WarehouseDto
{
    public string Name { get; set; }

    public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}