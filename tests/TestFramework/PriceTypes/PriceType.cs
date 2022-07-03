namespace TestFramework.PriceTypes;

public class PriceType
{
    public string Name { get; set; } = null!;

    public Guid ExternalId { get; set; }

    public bool DeletionMark { get; set; }

    public static PriceType Create(Guid externalId)
    {
        return new PriceType
        {
            ExternalId = externalId,
            Name = externalId.ToString(),
            DeletionMark = false
        };
    }
}