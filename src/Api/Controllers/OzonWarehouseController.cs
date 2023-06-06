using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.Warehouses.Commands;
using Infrastructure.MediatR.Ozon.Warehouses.Models;
using Infrastructure.MediatR.Ozon.Warehouses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/ozon/{ozonId:int}/warehouses")]
public class OzonWarehouseController : ControllerBase
{
    private readonly IMediator _mediator;

    public OzonWarehouseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> CreateOzonWarehouse([FromRoute] int ozonId,
        [FromBody] OzonWarehouseApplyModel model)
    {
        var command = new CreateOzonWarehouse.Command(ozonId, model);
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpGet]
    public async Task<Result> GetOzonWarehouses([FromRoute] int ozonId)
    {
        var query = new GetOzonWarehouses.Query(ozonId);
        var result = await _mediator.Send(query);
        return result;
    }
}