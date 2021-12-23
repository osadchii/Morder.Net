namespace Infrastructure.MediatR.ChangeTracking.Queries;

public class GetChangedStocksRequest
{
    public int MarketplaceId { get; set; }
    public int? Limit { get; set; }
}