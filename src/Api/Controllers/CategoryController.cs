using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.MediatR.Categories.Queries;
using Infrastructure.Models.Products;
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
    public async Task<Result<List<CategoryDto>>> GetAllCategories()
    {
        return (await _mediator.Send(new GetAllCategoriesRequest())).AsResult();
    }

    [HttpPost]
    public async Task<Result> UpdateCategory([Required] [FromBody] UpdateCategoryRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}