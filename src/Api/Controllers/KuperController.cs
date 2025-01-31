using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.Kuper.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Kuper.Clients.Orders;
using Integration.Kuper.Clients.Orders.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/kuper")]
public class KuperController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IKuperOrdersClient _kuperOrdersClient;
    private readonly MContext _context;
    private readonly IMapper _mapper;

    public KuperController(IMediator mediator, MContext context, IMapper mapper,
        IKuperOrdersClient kuperOrdersClient)
    {
        _mediator = mediator;
        _context = context;
        _mapper = mapper;
        _kuperOrdersClient = kuperOrdersClient;
    }

    [HttpPost]
    public async Task<Result> CreateMeso([Required] [FromBody] UpdateKuperRequest command)
    {
        command.Id = null;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    public async Task<Result> UpdateMeso([Required] [FromBody] UpdateKuperRequest command,
        [Required] int id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("runtest")]
    public async Task<Result> RunTest()
    {
        var marketplace = await _context.Marketplaces
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Type == MarketplaceType.Kuper);

        if (marketplace is null)
        {
            return new Result(ResultCode.Error).AddError("Kuper marketplace not found");
        }

        var kuper = _mapper.Map<KuperDto>(marketplace);

        using var httpClient = new HttpClient();

        var token = await _kuperOrdersClient.GetToken(kuper, httpClient);


        await Delay();
        
        var orders = await _kuperOrdersClient.GetOrders(kuper, token);
        await Delay();

        var firstOrder = orders.First();

        var notificationAccepted = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.accepted",
                Payload = new Payload
                {
                    OrderId = firstOrder.OriginalOrderId,
                    Number = "123456"
                }
            }
        };


        await _kuperOrdersClient.SendOrderNotification(kuper, notificationAccepted, token);
        await Delay();
        
        var notificationAtWork = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.in_work",
                Payload = new Payload
                {
                    OrderId = firstOrder.OriginalOrderId,
                    Number = "123456"
                }
            }
        };
        
        await _kuperOrdersClient.SendOrderNotification(kuper, notificationAtWork, token);
        // await Delay();
        //
        // var notificationAssembled = new KuperOrderNotification
        // {
        //     Event = new Event
        //     {
        //         Type = "order.assembled",
        //         Payload = new Payload
        //         {
        //             OrderId = firstOrder.OriginalOrderId,
        //             Number = "123456",
        //             Order = new Order
        //             {
        //                 OriginalOrderId = firstOrder.OriginalOrderId,
        //                 Positions = firstOrder.Positions.Select(x => new OrderPosition
        //                 {
        //                     Id = x.Id,
        //                     Quantity = x.Quantity
        //                 }).ToArray()
        //             }
        //         }
        //     }
        // };
        //
        // await _kuperOrdersClient.SendOrderNotification(kuper, notificationAssembled, token);
        await Delay();
        
        var notificationReadyForDelivery = new KuperOrderNotification
        {
            Event = new Event
            {
                Type = "order.ready_for_delivery",
                Payload = new Payload
                {
                    OrderId = firstOrder.OriginalOrderId,
                    Number = "123456",
                    Order = new Order
                    {
                        OriginalOrderId = firstOrder.OriginalOrderId,
                        Positions = firstOrder.Positions.Select(x => new OrderPosition
                        {
                            Id = x.Id,
                            Quantity = 1
                        }).ToArray()
                    }
                }
            }
        };
        
        await _kuperOrdersClient.SendOrderNotification(kuper, notificationReadyForDelivery, token);
        await Delay();
        
        var state = await _kuperOrdersClient.GetOrderState(kuper, firstOrder.OriginalOrderId, token);

        return state.AsResult();

        Task Delay() => Task.Delay(TimeSpan.FromSeconds(1));
    }
}