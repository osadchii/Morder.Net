using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class SetUserAdministratorRequest : IRequest<Unit>
{
    public int UserId { get; set; }
    public bool Administrator { get; set; }
}

public class SetUserAdministratorHandler : IRequestHandler<SetUserAdministratorRequest, Unit>
{
    private readonly MContext _context;

    public SetUserAdministratorHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetUserAdministratorRequest request, CancellationToken cancellationToken)
    {
        BotUser user = await _context.BotUsers
            .SingleAsync(u => u.Id == request.UserId, cancellationToken);

        user.Administrator = request.Administrator;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}