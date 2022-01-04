using Infrastructure.Models.Marketplaces;

namespace Integration.Common.Services.Orders;

public abstract class MarketplaceOrderLoader
{
    protected readonly Marketplace Marketplace;
    protected readonly IServiceProvider ServiceProvider;

    protected MarketplaceOrderLoader(Marketplace marketplace, IServiceProvider serviceProvider)
    {
        Marketplace = marketplace;
        ServiceProvider = serviceProvider;
    }

    public abstract Task LoadAsync();
}