using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Warehouses.Commands;
using Infrastructure.MediatR.Warehouses.Queries;
using Infrastructure.Models.Warehouses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/warehouse")]
public class WarehouseController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehouseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{externalId:guid}")]
    public async Task<Result> GetByExternalId([Required] Guid externalId)
    {
        WarehouseDto result = await _mediator.Send(new GetWarehouseByExternalIdRequest() { ExternalId = externalId });
        return result.AsResult();
    }

    [HttpPost]
    public async Task<Result> UpdateWarehouse([Required] [FromBody] UpdateWarehouseRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}