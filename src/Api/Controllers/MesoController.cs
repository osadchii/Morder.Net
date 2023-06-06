using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.Meso.Commands;
using Infrastructure.Models.Marketplaces.Meso;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/meso")]
public class MesoController : ControllerBase
{
    private readonly IMediator _mediator;

    public MesoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> CreateMeso([Required] [FromBody] UpdateMesoRequest command)
    {
        command.Id = null;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    public async Task<Result> UpdateMeso([Required] [FromBody] UpdateMesoRequest command,
        [Required] int id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }
}