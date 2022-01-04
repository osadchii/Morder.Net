using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Orders.Company.Commands;
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
}