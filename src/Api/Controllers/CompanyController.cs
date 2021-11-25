using System.ComponentModel.DataAnnotations;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.Models.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/company")]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CompanyDto>> GetCompanyInformation()
    {
        return Ok(await _mediator.Send(new GetCompanyInformation()));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCompany([Required] [FromBody] UpdateCompanyInformation command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}