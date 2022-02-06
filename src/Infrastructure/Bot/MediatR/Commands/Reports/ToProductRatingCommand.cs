using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Reports;

public class ToProductRatingCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
}

public class ToProductRatingHandler : IRequestHandler<ToProductRatingCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToProductRatingHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToProductRatingCommand request, CancellationToken cancellationToken)
    {
        Marketplace[] marketplaces = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.IsActive)
            .ToArrayAsync(cancellationToken);

        KeyboardBuilder menuBuilder = new KeyboardBuilder()
            .AddLine()
            .AddButton(MenuTexts.Back);

        foreach (Marketplace marketplace in marketplaces)
        {
            menuBuilder.AddLine();
            menuBuilder.AddButton($"{marketplace.Id} – {marketplace.Name}");
        }

        menuBuilder.AddLine()
            .AddButton(MenuTexts.Total);

        await _client.SendReplyKeyboard(request.ChatId, menuBuilder.Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.ProductRating), cancellationToken);

        return Unit.Value;
    }
}