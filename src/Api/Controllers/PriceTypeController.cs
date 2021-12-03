using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.PriceTypes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/pricetype")]
public class PriceTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public PriceTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> UpdatePriceType([Required] [FromBody] UpdatePriceTypeRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}