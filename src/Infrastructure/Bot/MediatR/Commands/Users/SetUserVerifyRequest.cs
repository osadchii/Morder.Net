using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class SetUserVerifyRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int UserId { get; set; }
    public bool Verified { get; set; }
}

public class SetUserVerifyHandler : IRequestHandler<SetUserVerifyRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public SetUserVerifyHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetUserVerifyRequest request, CancellationToken cancellationToken)
    {
        BotUser user = await _context.BotUsers
            .SingleAsync(u => u.Id == request.UserId, cancellationToken);

        user.Verified = request.Verified;
        await _context.SaveChangesAsync(cancellationToken);

        if (user.Verified && request.ChatId != user.ChatId)
        {
            await _mediator.Send(new ToMainMenuCommand()
            {
                ChatId = user.ChatId
            }, cancellationToken);
        }

        await _mediator.Send(new ToUserManagementCommand()
        {
            ChatId = request.ChatId,
            UserId = request.UserId
        }, cancellationToken);

        return Unit.Value;
    }
}