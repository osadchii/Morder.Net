using System.Globalization;
using AutoMapper;
using Infrastructure.MediatR.Marketplaces.Common.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using Integration.Common.Services.Feeds;
using Integration.Kuper.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Kuper.Feeds;

public class KuperFeedService : MarketplaceFeedService
{
    private readonly KuperDto _kuper;

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

        await SaveProductFeed(products, data, productImageService);
        await SaveCategoryFeed(products, data);
        await SaveStockFeed(products, data, _kuper.WarehouseExternalId.ToString());
    }

    private static async Task SaveStockFeed(IEnumerable<Product> products, MarketplaceProductData data, string warehouseId)
    {
        var stocks = products
            .Select(x =>
            {
                var price = data.GetProductPrice(x);
                return new KuperStockFeed.Item
                {
                    OfferId = x.Articul,
                    Stock = data.GetProductStock(x, price).ToString(CultureInfo.InvariantCulture),
                    OutletId = warehouseId
                };
            })
            .ToArray();

        var feed = new KuperStockFeed
        {
            Data = stocks
        };

        var feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
        var path = Path.Combine(feedPath, $"categories_{DateTime.UtcNow:yyyyMMddHHmm}.json");
        
        PrepareFeedFolder(feedPath, "categories_*.json");
        
        await feed.Save(path);
    }

    private static async Task SaveCategoryFeed(IEnumerable<Product> products, MarketplaceProductData data)
    {
        var categoryIds = products
            .Where(x => x.CategoryId.HasValue)
            .Select(x => x.CategoryId!.Value)
            .Distinct()
            .ToArray();

        var categories = data.Categories
            .Where(x => categoryIds.Contains(x.Key))
            .Select(x => new KuperCategoryFeed.Item
            {
                Id = x.Key.ToString(),
                Name = x.Value.Name
            })
            .ToArray();

        var feed = new KuperCategoryFeed
        {
            Data = categories
        };

        var feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
        var path = Path.Combine(feedPath, $"categories_{DateTime.UtcNow:yyyyMMddHHmm}.json");
        
        PrepareFeedFolder(feedPath, "categories_*.json");
        
        await feed.Save(path);
    }

    private static async Task SaveProductFeed(IEnumerable<Product> products, MarketplaceProductData data,
        IProductImageService productImageService)
    {
        var feed = new KuperProductFeed
        {
            Data = products
                .Select(x => x.ToKuperProduct(data, productImageService))
                .ToArray()
        };

        var feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
        var path = Path.Combine(feedPath, $"offers_{DateTime.UtcNow:yyyyMMddHHmm}.json");
        
        PrepareFeedFolder(feedPath, "offers_*.json");
        
        await feed.Save(path);
    }

    private static void PrepareFeedFolder(string feedPath, string removeFilesSearchPattern)
    {
        var files = Directory.GetFiles(feedPath, removeFilesSearchPattern);
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