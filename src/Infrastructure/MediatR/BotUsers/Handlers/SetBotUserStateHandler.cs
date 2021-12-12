using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.BotUsers.Handlers;

public class SetBotUserStateHandler : IRequestHandler<SetBotUserStateRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<SetBotUserStateHandler> _logger;

    public SetBotUserStateHandler(MContext context, ILogger<SetBotUserStateHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(SetBotUserStateRequest request, CancellationToken cancellationToken)
    {
        BotUser user = await _context.BotUsers
            .SingleAsync(u => u.ChatId == request.ChatId, cancellationToken);

        user.CurrentState = request.State;
        user.CurrentStateKey = request.StateKey;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}