using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.YandexMarket.Commands;
using Infrastructure.Models.Marketplaces.YandexMarket;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/yandexmarket")]
public class YandexMarketController : ControllerBase
{
    private readonly IMediator _mediator;

    public YandexMarketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> Create([Required] [FromBody] UpdateYandexMarketRequest command)
    {
        command.Id = null;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    public async Task<Result> Update([Required] [FromBody] UpdateYandexMarketRequest command,
        [Required] int id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }
}