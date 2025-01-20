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
            .Where(x => productsWIthImages.Contains(x.Id));

        var feed = new KuperProductFeed
        {
            Data = products
                .Select(x => x.ToKuperProduct(data, productImageService))
                .ToArray()
        };
        
        var feedPath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "feeds");
        var path = Path.Combine(feedPath, $"offers_{DateTime.UtcNow:yyyyMMddHHmm}.json");
        if (!Directory.Exists(feedPath))
        {
            Directory.CreateDirectory(feedPath);
        }

        await feed.Save(path);
    }
}