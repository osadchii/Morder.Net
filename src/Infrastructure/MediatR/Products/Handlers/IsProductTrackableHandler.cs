using Infrastructure.Extensions;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Products.Handlers;

public class IsProductTrackableHandler : IRequestHandler<IsProductTrackableRequest, bool>
{
    private readonly MContext _context;

    public IsProductTrackableHandler(MContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(IsProductTrackableRequest request, CancellationToken cancellationToken)
    {
        string productTypesStr = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == request.MarketplaceId)
            .Select(m => m.ProductTypes)
            .SingleAsync(cancellationToken);

        var productTypes = productTypesStr.FromJson<List<ProductType>>();

        return await _context.Products
            .AsNoTracking()
            .AnyAsync(p => !p.DeletionMark
                           && p.CategoryId.HasValue
                           && p.ProductType.HasValue
                           && productTypes.Contains(p.ProductType.Value), cancellationToken);
    }
}