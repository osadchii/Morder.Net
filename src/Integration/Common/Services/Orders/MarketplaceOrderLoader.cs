using AutoMapper;
using Infrastructure.Models.Marketplaces;

namespace Integration.Common.Services.Orders;

public abstract class MarketplaceOrderLoader
{
    protected readonly Marketplace Marketplace;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IMapper Mapper;
    protected readonly DateTime StartDate;

    protected MarketplaceOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper,
        DateTime startDate)
    {
        Marketplace = marketplace;
        ServiceProvider = serviceProvider;
        Mapper = mapper;
        StartDate = startDate;
    }

    public abstract Task LoadOrders();
}