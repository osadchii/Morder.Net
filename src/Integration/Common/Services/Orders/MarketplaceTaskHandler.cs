using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;

namespace Integration.Common.Services.Orders;

public abstract class MarketplaceTaskHandler
{
    protected readonly Marketplace Marketplace;
    protected readonly MarketplaceOrderTask OrderTask;
    protected readonly IServiceProvider ServiceProvider;
    protected Order Order => OrderTask.Order;

    protected MarketplaceTaskHandler(Marketplace marketplace, MarketplaceOrderTask task,
        IServiceProvider serviceProvider)
    {
        Marketplace = marketplace;
        OrderTask = task;
        ServiceProvider = serviceProvider;
    }

    public abstract Task Handle();
}