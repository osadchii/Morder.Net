using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.MarketplaceCategorySettings.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/marketplacecategorysetting")]
public class MarketplaceCategorySettingController : ControllerBase
{
    private readonly IMediator _mediator;

    public MarketplaceCategorySettingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result> UpdateSettings([Required] [FromBody] SetMarketplaceCategorySettingsRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}