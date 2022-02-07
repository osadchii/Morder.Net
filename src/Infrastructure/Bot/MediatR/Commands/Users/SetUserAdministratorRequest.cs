using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class SetUserAdministratorRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int UserId { get; set; }
    public bool Administrator { get; set; }
}

public class SetUserAdministratorHandler : IRequestHandler<SetUserAdministratorRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public SetUserAdministratorHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetUserAdministratorRequest request, CancellationToken cancellationToken)
    {
        BotUser user = await _context.BotUsers
            .SingleAsync(u => u.Id == request.UserId, cancellationToken);

        user.Administrator = request.Administrator;
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new ToUserManagementCommand()
        {
            ChatId = request.ChatId,
            UserId = request.UserId
        }, cancellationToken);

        return Unit.Value;
    }
}