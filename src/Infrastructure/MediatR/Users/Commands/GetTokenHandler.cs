using System.Net;
using Infrastructure.Common;
using Infrastructure.Models.Users;
using Infrastructure.Services.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.MediatR.Users.Commands;

public class GetTokenRequest : UserDto, IRequest<Result<TokenDto>>
{
}

public class GetTokenHandler : IRequestHandler<GetTokenRequest, Result<TokenDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public GetTokenHandler(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }
    
    public async Task<Result<TokenDto>> Handle(GetTokenRequest request, CancellationToken cancellationToken)
    {
        ApplicationUser identityUser = await _userManager.FindByNameAsync(request.Name);

        if (identityUser is null)
        {
            throw new HttpRequestException(string.Empty, null, HttpStatusCode.Forbidden);
        }

        var checkPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);

        if (!checkPassword)
        {
            throw new HttpRequestException(string.Empty, null, HttpStatusCode.Forbidden);
        }

        var token = new TokenDto
        {
            Token = _jwtTokenService.GetToken(identityUser)
        };

        return new Result<TokenDto>(token);
    }
}