namespace TestFramework.Warehouses;

public class Warehouse
{
    public string? Name { get; set; }

    public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }

    public static Warehouse Create(Guid externalId)
    {
        return new Warehouse
        {
            ExternalId = externalId,
            Name = externalId.ToString(),
            DeletionMark = false
        };
    }
}