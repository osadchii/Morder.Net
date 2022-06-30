using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.MediatR.Users.Commands;
using Infrastructure.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v1/user")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("register")]
    public Task<Result> RegisterUser([Required] [FromBody] RegisterUserRequest command)
    {
        return _mediator.Send(command);
    }

    [HttpPost]
    [Route("gettoken")]
    public Task<Result<TokenDto>> GetToken([Required] [FromBody] GetTokenRequest command)
    {
        return _mediator.Send(command);
    }
}