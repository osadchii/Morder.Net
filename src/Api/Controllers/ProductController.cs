using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
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
    public async Task<Result> GetAllProducts()
    {
        return (await _mediator.Send(new GetAllProductsRequest())).AsResult();
    }

    [HttpGet]
    [Route("{externalId:guid}")]
    public async Task<Result> GetByExternalId([Required] Guid externalId)
    {
        ProductDto? result = await _mediator.Send(new GetProductByExternalIdRequest { ExternalId = externalId });
        return result.AsResult();
    }

    [HttpGet]
    [Route("{articul}")]
    public async Task<Result> GetByArticul([Required] string articul)
    {
        ProductDto? result = await _mediator.Send(new GetProductByArticulRequest { Articul = articul });
        return result.AsResult();
    }

    [HttpPost]
    public Task<Result> UpdateProduct([Required] [FromBody] UpdateProductRequest command)
    {
        return _mediator.Send(command);
    }

    [HttpDelete]
    [Route("{externalId:guid}")]
    public async Task<Result> DeleteProductByExternalId([Required] Guid externalId)
    {
        return (await _mediator.Send(new DeleteProductByExternalIdRequest
        {
            ExternalId = externalId
        })).AsResult();
    }

    [HttpDelete]
    [Route("{articul}")]
    public async Task<Result> DeleteProductByArticul([Required] string articul)
    {
        return (await _mediator.Send(new DeleteProductByArticulRequest
        {
            Articul = articul
        })).AsResult();
    }
}