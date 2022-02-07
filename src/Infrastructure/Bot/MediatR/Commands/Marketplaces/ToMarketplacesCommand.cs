using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Marketplaces;

public class ToMarketplacesCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
}

public class ToMarketplacesHandler : IRequestHandler<ToMarketplacesCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToMarketplacesHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToMarketplacesCommand request, CancellationToken cancellationToken)
    {
        Marketplace[] marketplaces = await _context.Marketplaces
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        var menuBuilder = new KeyboardBuilder();

        foreach (Marketplace marketplace in marketplaces.OrderBy(u => u.Id))
        {
            menuBuilder.AddLine();
            menuBuilder.AddButton($"{marketplace.Id} – {marketplace.Name}");
        }

        await _client.SendReplyKeyboard(request.ChatId, menuBuilder.Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.MarketplacesMenu), cancellationToken);

        return Unit.Value;
    }
}