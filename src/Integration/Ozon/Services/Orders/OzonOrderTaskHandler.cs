using AutoMapper;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Orders;
using Integration.Ozon.Clients.Orders;
using Integration.Ozon.Clients.Orders.Messages;
using MediatR;
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
            TaskType.Sticker => HandleStickerTask(),
            _ => Task.CompletedTask
        };
    }

    private async Task HandleStickerTask()
    {
        var client = ServiceProvider.GetRequiredService<IOzonGetStickerClient>();
        byte[] file = await client.GetSticker(_ozonDto, new GetStickerRequest(Order.Number));

        var mediator = ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new SaveOrderStickerFromStringRequest()
        {
            Bytes = file,
            FileName = "Ozon Sticker.pdf",
            OrderId = Order.Id
        });
    }
}