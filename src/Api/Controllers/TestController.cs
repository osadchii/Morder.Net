using Infrastructure.Common;
using Infrastructure.Services.Marketplaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/test")]
public class TestController : ControllerBase
{
    private readonly IProductImageService _productImageService;

    public TestController(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }

    [HttpGet]
    public async Task<Result> GetAllProducts()
    {
        return (await _productImageService.GetProductIdsWithImages()).ToList().AsResult();
    }
}