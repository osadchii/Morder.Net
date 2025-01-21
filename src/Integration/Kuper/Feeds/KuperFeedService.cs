using AutoMapper;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Infrastructure.Services.Marketplaces;
using Integration.Common.Services.Feeds;
using Integration.Kuper.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Kuper.Feeds;

public class KuperFeedService : MarketplaceFeedService
{
    private readonly KuperDto _kuper;
    private readonly string _feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");

    public KuperFeedService(IMapper mapper, IServiceProvider serviceProvider, Marketplace marketplace) : base(mapper,
        serviceProvider, marketplace)
    {
        _kuper = mapper.Map<KuperDto>(Marketplace);
    }

    public override async Task GenerateFeed()
    {
        var productImageService = ServiceProvider.GetRequiredService<IProductImageService>();
        var mediator = ServiceProvider.GetRequiredService<IMediator>();

        var data = await mediator.Send(new GetMarketplaceProductDataRequest
        {
            MarketplaceId = _kuper.Id
        });

        var productsWIthImages = await productImageService.GetProductIdsWithImages();
        var products = data.Products
            .Where(x => productsWIthImages.Contains(x.Id))
            .ToArray();
        
        var warehouseId = _kuper.WarehouseExternalId.ToString();
        
        await SaveFeed(() => KuperProductFeed.Build(products, data, productImageService), "offers");
        await SaveFeed(() => KuperCategoryFeed.Build(products, data), "categories");
        await SaveFeed(() => KuperStockFeed.Build(products, data, warehouseId), "stock");
        await SaveFeed(() => KuperPriceFeed.Build(products, data, warehouseId), "offer_prices");
    }

    private string GetFeedFilePath(string feedName)
    {
        return Path.Combine(_feedPath, $"{feedName}_{DateTime.UtcNow:yyyyMMddHHmm}.json");
    }
    
    private async Task SaveFeed<T>(Func<KuperFeed<T>> generateFeed, string feedName)
    {
        var feed = generateFeed();
        var path = GetFeedFilePath(feedName);

        PrepareFeedFolder(_feedPath, feedName);

        await feed.Save(path);
    }

    private static void PrepareFeedFolder(string feedPath, string feedName)
    {
        var files = Directory.GetFiles(feedPath, $"{feedName}_*.json");
        foreach (var file in files)
        {
            File.Delete(file);
        }

        if (!Directory.Exists(feedPath))
        {
            Directory.CreateDirectory(feedPath);
        }
    }
}