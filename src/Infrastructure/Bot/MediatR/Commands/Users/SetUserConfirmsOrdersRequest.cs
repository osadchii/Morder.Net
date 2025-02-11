using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bot.MediatR.Commands.Users;

public class SetUserConfirmsOrdersRequest : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int UserId { get; set; }
    public bool ConfirmsOrders { get; set; }
}

public class SetUserConfirmsOrdersHandler : IRequestHandler<SetUserConfirmsOrdersRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public SetUserConfirmsOrdersHandler(MContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(SetUserConfirmsOrdersRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.BotUsers
            .SingleAsync(u => u.Id == request.UserId, cancellationToken);

        user.ConfirmsOrders = request.ConfirmsOrders;
        await _context.SaveChangesAsync(cancellationToken);

        await _mediator.Send(new ToUserManagementCommand
        {
            ChatId = request.ChatId,
            UserId = request.UserId
        }, cancellationToken);

        return Unit.Value;
    }
}