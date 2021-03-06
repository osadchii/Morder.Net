using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.MediatR.Companies.Queries;
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
    public async Task<Result> GetCompanyInformation()
    {
        return (await _mediator.Send(new GetCompanyInformationRequest())).AsResult();
    }

    [HttpPost]
    public async Task<Result> UpdateCompany([Required] [FromBody] UpdateCompanyInformationRequest command)
    {
        await _mediator.Send(command);
        return Result.Ok;
    }
}