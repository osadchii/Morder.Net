using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Reports;

public class ToSoldProductCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
}

public class ToSoldProductsHandler : IRequestHandler<ToSoldProductCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToSoldProductsHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToSoldProductCommand request, CancellationToken cancellationToken)
    {
        var marketplaces = await _context.Marketplaces
            .AsNoTracking()
            .Where(m => m.IsActive)
            .ToArrayAsync(cancellationToken);

        var menuBuilder = new KeyboardBuilder()
            .AddLine()
            .AddButton(MenuTexts.Back);

        foreach (var marketplace in marketplaces.OrderBy(m => m.Id))
        {
            menuBuilder.AddLine();
            menuBuilder.AddButton($"{marketplace.Id} – {marketplace.Name}");
        }

        menuBuilder.AddLine()
            .AddButton(MenuTexts.Total);

        await _client.SendReplyKeyboard(request.ChatId, menuBuilder.Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.SoldProducts), cancellationToken);

        return Unit.Value;
    }
}