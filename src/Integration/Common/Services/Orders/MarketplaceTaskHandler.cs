using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Common.Services.Orders;

public abstract class MarketplaceTaskHandler
{
    protected readonly Marketplace Marketplace;
    protected readonly MarketplaceOrderTask OrderTask;
    protected readonly IServiceProvider ServiceProvider;
    protected Order Order => OrderTask.Order;
    protected IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();

    protected MarketplaceTaskHandler(Marketplace marketplace, MarketplaceOrderTask task,
        IServiceProvider serviceProvider)
    {
        Marketplace = marketplace;
        OrderTask = task;
        ServiceProvider = serviceProvider;
    }

    public abstract Task Handle();
}