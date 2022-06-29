using Infrastructure.Extensions;
using Infrastructure.MediatR.MarketplaceCategorySettings.Commands;
using Infrastructure.Models.MarketplaceCategorySettings;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.MarketplaceCategorySettings.Handlers;

public class SetMarketplaceCategorySettingsHandler : IRequestHandler<SetMarketplaceCategorySettingsRequest, Unit>
{
    private readonly MContext _context;
    private readonly IChangeTrackingService _changeTrackingService;

    public SetMarketplaceCategorySettingsHandler(MContext context,
        IChangeTrackingService changeTrackingService)
    {
        _context = context;
        _changeTrackingService = changeTrackingService;
    }

    public async Task<Unit> Handle(SetMarketplaceCategorySettingsRequest request, CancellationToken cancellationToken)
    {
        List<Category> categories;
        if (request.Recursive)
        {
#pragma warning disable CS8620
            categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == request.CategoryId)
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ThenInclude(c => c.Children)
                .ToListAsync(cancellationToken);
#pragma warning restore CS8620
        }
        else
        {
            categories = await _context.Categories
                .AsNoTracking()
                .Where(c => c.Id == request.CategoryId)
                .ToListAsync(cancellationToken);
        }

        List<int> categoryIds = new();
        FillCategoryIds(categories, categoryIds);

        List<MarketplaceCategorySetting> settings = await _context.MarketplaceCategorySettings
            .Where(s => s.MarketplaceId == request.MarketplaceId
                        && categoryIds.Contains(s.CategoryId))
            .ToListAsync(cancellationToken);

        foreach (MarketplaceCategorySetting setting in settings)
        {
            setting.Blocked = request.Blocked!.Value;
        }

        IEnumerable<int> newIds = categoryIds.Except(settings.Select(s => s.CategoryId));
        await _context.AddRangeAsync(newIds.Select(id => new MarketplaceCategorySetting
        {
            MarketplaceId = request.MarketplaceId!.Value,
            CategoryId = id,
            Blocked = request.Blocked!.Value
        }), cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        await TrackProductStocksByCategoryIds(request.MarketplaceId!.Value, categoryIds, cancellationToken);

        return Unit.Value;
    }

    private async Task TrackProductStocksByCategoryIds(int marketplaceId, ICollection<int> categoryIds,
        CancellationToken cancellationToken)
    {
        Marketplace marketplace = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.Id == marketplaceId)
            .SingleAsync(cancellationToken);

        var productTypes = marketplace.ProductTypes.FromJson<List<ProductType>>();

        List<int> productIds = await _context.Products
            .AsNoTracking()
            .Where(p => !p.DeletionMark
                        && p.CategoryId.HasValue
                        && categoryIds.Contains(p.CategoryId.Value)
                        && p.ProductType.HasValue
                        && productTypes!.Contains(p.ProductType!.Value))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        await _changeTrackingService.TrackStocksChange(marketplaceId, productIds, cancellationToken);
    }

    private static void FillCategoryIds(ICollection<Category>? categories, ICollection<int> result)
    {
        if (categories is null)
        {
            return;
        }

        foreach (Category category in categories)
        {
            result.Add(category.Id);
            FillCategoryIds(category.Children, result);
        }
    }
}