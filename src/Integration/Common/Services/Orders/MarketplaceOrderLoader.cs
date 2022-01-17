using AutoMapper;
using Infrastructure.Models.Marketplaces;

namespace Integration.Common.Services.Orders;

public abstract class MarketplaceOrderLoader
{
    protected readonly Marketplace Marketplace;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IMapper Mapper;

    protected MarketplaceOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper)
    {
        Marketplace = marketplace;
        ServiceProvider = serviceProvider;
        Mapper = mapper;
    }

    public abstract Task LoadAsync();
}