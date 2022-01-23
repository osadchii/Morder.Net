using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services.Orders;

public class OzonOrderTaskHandler : MarketplaceTaskHandler
{
    private readonly OzonDto _ozonDto;

    public OzonOrderTaskHandler(Marketplace marketplace, MarketplaceOrderTask task, IServiceProvider serviceProvider) :
        base(marketplace, task, serviceProvider)
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();

        _ozonDto = mapper.Map<OzonDto>(Marketplace);
    }

    public override Task Handle()
    {
        return OrderTask.Type switch
        {
            TaskType.Pack => Task.CompletedTask,
            TaskType.Ship => Task.CompletedTask,
            TaskType.Reject => Task.CompletedTask,
            TaskType.Sticker => Task.CompletedTask,
            _ => Task.CompletedTask
        };
    }
}