using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Commands;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Models;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/ozon/{ozonId:int}/warehouses/{ozonWarehouseId:int}/blacklist")]
public class OzonWarehouseBlacklistController : ControllerBase
{
    private readonly IMediator _mediator;

    public OzonWarehouseBlacklistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> AddProductToBlacklist([FromRoute] int ozonId, [FromRoute] int ozonWarehouseId,
        [FromBody] OzonWarehouseBlacklistApplyModel model)
    {
        var command = new CreateOzonWarehouseBlacklist.Command(ozonId, ozonWarehouseId, model);
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpDelete("{productId:int}")]
    public async Task<Result> AddProductToBlacklist([FromRoute] int ozonId, [FromRoute] int ozonWarehouseId,
        [FromRoute] int productId)
    {
        var command = new DeleteOzonWarehouseBlacklist.Command(ozonId, ozonWarehouseId, productId);
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpGet]
    public async Task<Result> GetBlacklist([FromRoute] int ozonId, [FromRoute] int ozonWarehouseId)
    {
        var query = new GetOzonWarehouseBlacklists.Query(ozonId, ozonWarehouseId);
        var result = await _mediator.Send(query);
        return result;
    }
}