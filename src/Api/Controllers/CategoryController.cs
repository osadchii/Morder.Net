using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.MediatR.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<Result> GetAllCategories()
    {
        return (await _mediator.Send(new GetAllCategoriesRequest())).AsResult();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<Result> GetCategoryById([Required] int id)
    {
        return (await _mediator.Send(new GetCategoryByIdRequest()
        {
            Id = id
        })).AsResult();
    }

    [HttpGet]
    [Route("{externalId:guid}")]
    public async Task<Result> GetCategoryByExternalId([Required] Guid externalId)
    {
        return (await _mediator.Send(new GetCategoryByExternalIdRequest()
        {
            ExternalId = externalId
        })).AsResult();
    }

    [HttpPost]
    public async Task<Result> UpdateCategory([Required] [FromBody] UpdateCategoryRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }

    [HttpPost]
    [Route("children")]
    public async Task<Result> GetChildrenCategories(
        [Required] [FromBody] GetChildrenCategoriesByParentIdRequest request)
    {
        return (await _mediator.Send(request)).AsResult();
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<Result> DeleteCategoryById([Required] int id)
    {
        return (await _mediator.Send(new DeleteCategoryByIdRequest()
        {
            Id = id
        })).AsResult();
    }

    [HttpDelete]
    [Route("{externalId:guid}")]
    public async Task<Result> DeleteCategoryByExternalId([Required] Guid externalId)
    {
        return (await _mediator.Send(new DeleteCategoryByExternalIdRequest()
        {
            ExternalId = externalId
        })).AsResult();
    }
}