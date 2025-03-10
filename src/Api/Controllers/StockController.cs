using System.ComponentModel.DataAnnotations;
using Api.Extensions;
using Infrastructure.Common;
using Infrastructure.MediatR.Stocks.Commands;
using Infrastructure.MediatR.Stocks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/stock")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Result>> UpdateStock([Required] [FromBody] UpdateStockRequest command)
    {
        return (await _mediator.Send(command)).ToActionResult();
    }

    [Route("{warehouseExternalId:guid}")]
    [HttpGet]
    public async Task<ActionResult<Result>> GetAllStocks([Required] Guid warehouseExternalId)
    {
        return (await _mediator.Send(new GetAllStocksRequest
        {
            WarehouseExternalId = warehouseExternalId
        }));
    }
}