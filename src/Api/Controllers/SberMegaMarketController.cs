using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Queries;
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
    public async Task<Result> CreateSberMegaMarket([Required] [FromBody] UpdateSberMegaMarketRequest command)
    {
        command.Id = null;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    public async Task<Result> UpdateSberMegaMarket([Required] [FromBody] UpdateSberMegaMarketRequest command,
        [Required] int id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Result> GetSberMegaMarketById([Required] int id)
    {
        var result = await _mediator.Send(new GetSberMegaMarketByIdRequest()
        {
            Id = id
        });
        return result.AsResult();
    }
}