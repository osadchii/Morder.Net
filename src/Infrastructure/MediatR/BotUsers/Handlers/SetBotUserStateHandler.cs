using Infrastructure.MediatR.BotUsers.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.BotUsers.Handlers;

public class SetBotUserStateHandler : IRequestHandler<SetBotUserStateRequest, Unit>
{
    private readonly MContext _context;

    public SetBotUserStateHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetBotUserStateRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.BotUsers
            .SingleAsync(u => u.ChatId == request.ChatId, cancellationToken);

        user.CurrentState = request.State;
        user.CurrentStateKey = request.StateKey;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}