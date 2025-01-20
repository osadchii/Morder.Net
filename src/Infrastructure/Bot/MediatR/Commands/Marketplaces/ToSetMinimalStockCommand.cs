using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using MediatR;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Marketplaces;

public class ToSetMinimalStockCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int MarketplaceId { get; set; }
}

public class ToSetMinimalStockHandler : IRequestHandler<ToSetMinimalStockCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly IMediator _mediator;

    public ToSetMinimalStockHandler(ITelegramBotClient client, IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToSetMinimalStockCommand request, CancellationToken cancellationToken)
    {
        var menuBuilder = new KeyboardBuilder()
            .AddLine()
            .AddButton(MenuTexts.Back)
            .Build();

        await _client.SendReplyKeyboard(request.ChatId, menuBuilder);
        await _client.SendTextAsync(request.ChatId, MessageConstants.EnterNewMinimalStock);

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.SetMinimalStock,
            request.MarketplaceId.ToString()), cancellationToken);

        return Unit.Value;
    }
}