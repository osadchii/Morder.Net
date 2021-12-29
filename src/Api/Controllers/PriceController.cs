using System.ComponentModel.DataAnnotations;
using Api.Extensions;
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
    public async Task<ActionResult<Result>> UpdateStock([Required] [FromBody] UpdatePriceRequest command)
    {
        return (await _mediator.Send(command)).ToActionResult();
    }
}