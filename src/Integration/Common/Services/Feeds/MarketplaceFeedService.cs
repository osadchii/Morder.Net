using AutoMapper;
using Infrastructure.Models.Marketplaces;

namespace Integration.Common.Services.Feeds;

public abstract class MarketplaceFeedService
{
    protected readonly IMapper Mapper;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly Marketplace Marketplace;

    protected MarketplaceFeedService(IMapper mapper, IServiceProvider serviceProvider, Marketplace marketplace)
    {
        Mapper = mapper;
        ServiceProvider = serviceProvider;
        Marketplace = marketplace;
    }

    public abstract Task GenerateFeed();
}