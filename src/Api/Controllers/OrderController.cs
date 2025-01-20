using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.MediatR.Orders.Company.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/order")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("confirm")]
    public async Task<Result> Confirm([Required] [FromBody] ConfirmOrderRequest request)
    {
        await _mediator.Send(request);
        return Result.Ok;
    }

    [HttpPost]
    [Route("pack")]
    public async Task<Result> Pack([Required] [FromBody] PackOrderRequest request)
    {
        await _mediator.Send(request);
        return Result.Ok;
    }

    [HttpPost]
    [Route("ship")]
    public async Task<Result> Ship([Required] [FromBody] ShipOrderRequest request)
    {
        await _mediator.Send(request);
        return Result.Ok;
    }

    [HttpPost]
    [Route("reject")]
    public async Task<Result> Reject([Required] [FromBody] RejectOrderRequest request)
    {
        await _mediator.Send(request);
        return Result.Ok;
    }

    [HttpGet]
    [Route("sticker/{externalId:guid}")]
    public async Task<IActionResult> Sticker([Required] Guid externalId)
    {
        var stickerData = await _mediator.Send(new GetOrderStickerRequest()
        {
            ExternalId = externalId
        });
        return File(stickerData.StickerData, "application/octet-stream", stickerData.Name);
    }

    [HttpGet]
    [Route("{externalId:guid}")]
    public async Task<Result> GetOrderByExternalId([Required] Guid externalId)
    {
        return await _mediator.Send(new GetOrderByExternalIdRequest()
        {
            ExternalId = externalId
        });
    }

    [HttpPost]
    [Route("changes")]
    public Task<Result> GetChangedOrders([Required] [FromBody] GetChangedOrdersRequest request)
    {
        return _mediator.Send(request);
    }
}