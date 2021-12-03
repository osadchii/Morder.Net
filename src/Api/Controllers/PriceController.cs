using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Prices.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/price")]
public class PriceController : ControllerBase
{
    private readonly IMediator _mediator;

    public PriceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> UpdateStock([Required] [FromBody] UpdatePriceRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}