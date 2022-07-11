using AutoMapper;
using Infrastructure.Extensions;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Marketplaces.TaskContext;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using Integration.Common.Services.Orders;
using Integration.Ozon.Clients.Orders;
using Integration.Ozon.Clients.Orders.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services.Orders;

public class OzonOrderTaskHandler : MarketplaceTaskHandler
{
    private readonly OzonDto _ozonDto;
    private readonly IProductIdentifierService _identifierService;

    public OzonOrderTaskHandler(Marketplace marketplace, MarketplaceOrderTask task, IServiceProvider serviceProvider) :
        base(marketplace, task, serviceProvider)
    {
        var mapper = ServiceProvider.GetRequiredService<IMapper>();
        _identifierService = ServiceProvider.GetRequiredService<IProductIdentifierService>();

        _ozonDto = mapper.Map<OzonDto>(Marketplace);
    }

    public override Task Handle()
    {
        return OrderTask.Type switch
        {
            TaskType.Pack => HandlePackOrderTask(),
            TaskType.Reject => HandleRejectOrderTask(),
            TaskType.Sticker => HandleStickerTask(),
            _ => Task.CompletedTask
        };
    }

    private async Task HandlePackOrderTask()
    {
        var client = ServiceProvider.GetRequiredService<IOzonPackOrderClient>();

        IEnumerable<int> productIds = Order.Boxes.Select(b => b.ProductId).Distinct();
        Dictionary<int, string> externalIds = await _identifierService.GetIdentifiersAsync(_ozonDto.Id, productIds, ProductIdentifierType.OzonFbs);

        var request = new PackPostingRequest
        {
            PostingNumber = Order.Number,
            Packages = Order.Boxes.GroupBy(b => b.Number).Select(g => new PackPostingPackage()
            {
                Products = g.Select(p => new PackPostingProduct()
                {
                    Quantity = Convert.ToInt32(p.Count),
                    ProductId = Convert.ToInt32(externalIds[p.ProductId])
                })
            })
        };

        await client.PackOrder(_ozonDto, request);
    }

    private async Task HandleRejectOrderTask()
    {
        var client = ServiceProvider.GetRequiredService<IOzonRejectOrderClient>();

        var taskContext = OrderTask.TaskContext!.FromJson<RejectOrderContext>()!;
        
        IEnumerable<int> productIds = taskContext.Items.Select(b => b.ProductId).Distinct();
        Dictionary<int, string> externalIds = await _identifierService.GetIdentifiersAsync(_ozonDto.Id, productIds, ProductIdentifierType.OzonFbs);
        
        await client.RejectOrder(_ozonDto, new RejectPostingRequest()
        {
            PostingNumber = Order.Number,
            Items = taskContext.Items.Select(i => new RejectPostingItem
            {
                Quantity = Convert.ToInt32(i.Count),
                Sku = Convert.ToInt32(externalIds[i.ProductId])
            })
        });
    }

    private async Task HandleStickerTask()
    {
        var client = ServiceProvider.GetRequiredService<IOzonGetStickerClient>();
        byte[] file = await client.GetSticker(_ozonDto, new GetStickerRequest(Order.Number));

        await Mediator.Send(new SaveOrderStickerFromStringRequest()
        {
            Bytes = file,
            FileName = "Ozon Sticker.pdf",
            OrderId = Order.Id
        });
    }
}