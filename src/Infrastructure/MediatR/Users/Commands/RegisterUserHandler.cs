using Infrastructure.Common;
using Infrastructure.Models.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.MediatR.Users.Commands;

public class RegisterUserRequest : UserDto, IRequest<Result>
{
}

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _userManager.CreateAsync(new ApplicationUser
        {
            UserName = request.Name,
            Name = request.Name
        }, request.Password);

        if (result.Succeeded)
        {
            return Result.Ok;
        }

        var resultValue = new Result(ResultCode.Error);
        resultValue.AddError(string.Join(',', result.Errors.Select(e => e.Description)));
        
        return resultValue;
    }
}