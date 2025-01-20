using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Warehouses.Commands;
using Infrastructure.MediatR.Warehouses.Queries;
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
    public async Task<Result> GetAllWarehouses()
    {
        return (await _mediator.Send(new GetAllWarehousesRequest())).AsResult();
    }

    [HttpGet]
    [Route("{externalId:guid}")]
    public async Task<Result> GetByExternalId([Required] Guid externalId)
    {
        var result = await _mediator.Send(new GetWarehouseByExternalIdRequest() { ExternalId = externalId });
        return result.AsResult();
    }

    [HttpPost]
    public async Task<Result> UpdateWarehouse([Required] [FromBody] UpdateWarehouseRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }

    [HttpDelete]
    [Route("{externalId:guid}")]
    public async Task<Result> DeleteWarehouseByExternalId([Required] Guid externalId)
    {
        return (await _mediator.Send(new DeleteWarehouseByExternalIdRequest()
        {
            ExternalId = externalId
        })).AsResult();
    }
}