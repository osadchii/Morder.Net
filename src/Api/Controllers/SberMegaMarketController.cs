using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/sbermegamarket")]
public class SberMegaMarketController : ControllerBase
{
    private readonly IMediator _mediator;

    public SberMegaMarketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> UpdateSberMegaMarket([Required] [FromBody] UpdateSberMegaMarketRequest command)
    {
        SberMegaMarketDto result = await _mediator.Send(command);
        return result.AsResult();
    }
}