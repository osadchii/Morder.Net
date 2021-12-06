using Infrastructure.Models.Prices;
using MediatR;

namespace Infrastructure.MediatR.PriceTypes.Queries;

public class GetPriceTypeByExternalIdRequest : IRequest<PriceTypeDto?>
{
    public Guid ExternalId { get; set; }
}