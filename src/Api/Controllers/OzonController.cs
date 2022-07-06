using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.Ozon.Commands;
using Infrastructure.MediatR.Marketplaces.Ozon.Queries;
using Infrastructure.Models.Marketplaces.Ozon;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/ozon")]
public class OzonController : ControllerBase
{
    private readonly IMediator _mediator;

    public OzonController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> CreateOzon([Required] [FromBody] UpdateOzonRequest command)
    {
        command.Id = null;
        OzonDto result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    public async Task<Result> UpdateOzon([Required] [FromBody] UpdateOzonRequest command,
        [Required] int id)
    {
        command.Id = id;
        OzonDto result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Result> GetOzonById([Required] int id)
    {
        OzonDto result = await _mediator.Send(new GetOzonByIdRequest()
        {
            Id = id
        });
        return result.AsResult();
    }
}