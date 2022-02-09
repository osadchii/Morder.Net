using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class RemoveUserRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int UserId { get; set; }
}

public class RemoveUserHandler : IRequestHandler<RemoveUserRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public RemoveUserHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
    {
        BotUser user = await _context.BotUsers
            .SingleAsync(u => u.Id == request.UserId, cancellationToken);

        user.IsDeleted = true;
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new ToUsersCommand()
        {
            ChatId = request.ChatId
        }, cancellationToken);

        return Unit.Value;
    }
}