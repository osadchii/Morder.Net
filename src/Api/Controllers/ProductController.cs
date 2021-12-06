using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.Enums;
using Infrastructure.MediatR.Products.Commands;
using Infrastructure.MediatR.Products.Queries;
using Infrastructure.Models.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/product")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{externalId:guid}")]
    public async Task<Result> GetByExternalId([Required] Guid externalId)
    {
        ProductDto? result = await _mediator.Send(new GetProductByExternalIdRequest { ExternalId = externalId });
        return result.AsResult();
    }

    [HttpPost]
    public async Task<Result> UpdateProduct([Required] [FromBody] UpdateProductRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}