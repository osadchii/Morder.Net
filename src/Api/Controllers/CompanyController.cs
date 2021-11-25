using System.ComponentModel.DataAnnotations;
using Infrastructure.MediatR.Companies.Commands;
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

    [HttpPut]
    public async Task<IActionResult> UpdateCompany([Required] [FromBody] UpdateCompanyInformation command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}