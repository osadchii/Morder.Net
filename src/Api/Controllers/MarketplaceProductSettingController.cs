using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/marketplaceproductsetting")]
public class MarketplaceProductSettingController : ControllerBase
{
    private readonly IMediator _mediator;

    public MarketplaceProductSettingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> UpdateSettings([Required] [FromBody] SetMarketplaceProductSettingsRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}