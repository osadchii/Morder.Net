using Infrastructure.Extensions;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Marketplaces.Common.Handlers;

public class GetMarketplaceProductHandler : IRequestHandler<GetMarketplaceProductDataRequest, MarketplaceProductData>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public GetMarketplaceProductHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<MarketplaceProductData> Handle(GetMarketplaceProductDataRequest request,
        CancellationToken cancellationToken)
    {
        var company = await _mediator.Send(new GetCompanyInformationRequest(), cancellationToken);

        if (!company.PriceTypeId.HasValue)
        {
            throw new Exception("Base price type for the company has not been set");
        }

        var marketplace =
            await _context.Marketplaces
                .AsNoTracking()
                .FirstAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        var productTypes = marketplace.ProductTypes.FromJson<List<ProductType>>()!;

        var products = await _context.Products.AsNoTracking()
            .Where(p => p.ProductType.HasValue && productTypes.Contains(p.ProductType.Value) && !p.DeletionMark &&
                        p.CategoryId.HasValue)
            .ToListAsync(cancellationToken);

        var categories = await _context.Categories
            .AsNoTracking()
            .ToDictionaryAsync(c => c.Id, c => c, cancellationToken);

        var stocks = await _context.Stocks
            .AsNoTracking()
            .Where(s => s.WarehouseId == marketplace.WarehouseId)
            .Select(s => new { s.ProductId, s.Value })
            .ToDictionaryAsync(s => s.ProductId, s => s.Value, cancellationToken);

        var basePrices = await _context.Prices
            .AsNoTracking()
            .Where(p => p.PriceTypeId == company.PriceTypeId.Value)
            .Select(p => new { p.ProductId, p.Value })
            .ToDictionaryAsync(p => p.ProductId, p => p.Value, cancellationToken);

        Dictionary<int, decimal> specialPrices;

        if (marketplace.PriceTypeId.HasValue)
        {
            specialPrices = await _context.Prices
                .AsNoTracking()
                .Where(p => p.PriceTypeId == marketplace.PriceTypeId.Value)
                .Select(p => new { p.ProductId, p.Value })
                .ToDictionaryAsync(p => p.ProductId, p => p.Value, cancellationToken);
        }
        else
        {
            specialPrices = new Dictionary<int, decimal>();
        }

        var categorySettings = await _context.MarketplaceCategorySettings
            .AsNoTracking()
            .Where(s => s.MarketplaceId == marketplace.Id)
            .ToDictionaryAsync(s => s.CategoryId, s => s, cancellationToken);

        var productSettings = await _context.MarketplaceProductSettings
            .AsNoTracking()
            .Where(s => s.MarketplaceId == marketplace.Id)
            .ToDictionaryAsync(s => s.ProductId, s => s, cancellationToken);

        return new MarketplaceProductData(marketplace,
            products, categories, stocks, basePrices, specialPrices, categorySettings, productSettings);
    }
}