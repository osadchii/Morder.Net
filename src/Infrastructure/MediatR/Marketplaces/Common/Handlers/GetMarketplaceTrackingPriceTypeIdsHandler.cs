using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.Models.Companies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class
    GetMarketplaceTrackingPriceTypeIdsHandler : IRequestHandler<GetMarketplaceTrackingPriceTypeIdsRequest, List<int>>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public GetMarketplaceTrackingPriceTypeIdsHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<List<int>> Handle(GetMarketplaceTrackingPriceTypeIdsRequest request,
        CancellationToken cancellationToken)
    {
        CompanyDto company = await _mediator.Send(new GetCompanyInformationRequest(), cancellationToken);
        int? priceTypeId = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == request.MarketplaceId && m.PriceTypeId.HasValue)
            .Select(m => m.PriceTypeId)
            .FirstOrDefaultAsync(cancellationToken);

        var result = new List<int>();

        if (company.PriceTypeId.HasValue)
        {
            result.Add(company.PriceTypeId.Value);
        }

        if (priceTypeId.HasValue)
        {
            result.Add(priceTypeId.Value);
        }

        return result;
    }
}