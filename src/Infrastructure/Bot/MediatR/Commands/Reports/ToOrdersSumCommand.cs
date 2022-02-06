using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Reports;

public class ToOrdersSumCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
}

public class ToOrdersSumHandler : IRequestHandler<ToOrdersSumCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToOrdersSumHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToOrdersSumCommand request, CancellationToken cancellationToken)
    {
        DateTime startDate = await _context.Orders.AsNoTracking()
            .MinAsync(o => o.Date, cancellationToken);

        await _client.SendReplyKeyboard(request.ChatId, new KeyboardBuilder()
            .AddLine()
            .AddButton(MenuTexts.Back)
            .AddLine()
            .AddDateIntervalButtons(startDate)
            .Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.OrdersSum), cancellationToken);

        return Unit.Value;
    }
}