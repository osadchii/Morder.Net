using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.PriceTypes.Commands;
using Infrastructure.MediatR.PriceTypes.Queries;
using Infrastructure.Models.Prices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/pricetype")]
public class PriceTypeController : ControllerBase
{
    private readonly IMediator _mediator;

    public PriceTypeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<Result> GetAllPriceTypes()
    {
        return (await _mediator.Send(new GetAllPriceTypesRequest())).AsResult();
    }

    [HttpGet]
    [Route("{externalId:guid}")]
    public async Task<Result> GetByExternalId([Required] Guid externalId)
    {
        PriceTypeDto? result = await _mediator.Send(new GetPriceTypeByExternalIdRequest() { ExternalId = externalId });
        return result.AsResult();
    }

    [HttpPost]
    public async Task<Result> UpdatePriceType([Required] [FromBody] UpdatePriceTypeRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }

    [HttpDelete]
    [Route("{externalId:guid}")]
    public async Task<Result> DeletePriceTypeByExternalId([Required] Guid externalId)
    {
        return (await _mediator.Send(new DeletePriceTypeByExternalIdRequest()
        {
            ExternalId = externalId
        })).AsResult();
    }
}