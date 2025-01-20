using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Reports;

public class ToProductRatingByMarketplaceCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int? MarketplaceId { get; set; }
}

public class ToProductRatingByMarketplaceHandler : IRequestHandler<ToProductRatingByMarketplaceCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToProductRatingByMarketplaceHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToProductRatingByMarketplaceCommand request, CancellationToken cancellationToken)
    {
        var startDate = await _context.Orders.AsNoTracking()
            .MinAsync(o => o.Date, cancellationToken);

        await _client.SendReplyKeyboard(request.ChatId, new KeyboardBuilder()
            .AddLine()
            .AddButton(MenuTexts.Back)
            .AddLine()
            .AddDateIntervalButtons(startDate)
            .Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.ProductRatingByMarketplace,
            request.MarketplaceId.HasValue ? request.MarketplaceId.ToString() : null), cancellationToken);

        return Unit.Value;
    }
}