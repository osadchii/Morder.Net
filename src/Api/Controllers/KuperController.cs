using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Marketplaces.Kuper.Commands;
using Infrastructure.MediatR.Marketplaces.Meso.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/kuper")]
public class KuperController : ControllerBase
{
    private readonly IMediator _mediator;

    public KuperController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> CreateMeso([Required] [FromBody] UpdateKuperRequest command)
    {
        command.Id = null;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }

    [HttpPost]
    [Route("{id:int}")]
    public async Task<Result> UpdateMeso([Required] [FromBody] UpdateKuperRequest command,
        [Required] int id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return result.AsResult();
    }
}