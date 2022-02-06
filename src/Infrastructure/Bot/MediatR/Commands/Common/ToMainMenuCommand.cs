using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using MediatR;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Common;

public class ToMainMenuCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
}

public class ToMainMenuHandler : IRequestHandler<ToMainMenuCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly IMediator _mediator;

    public ToMainMenuHandler(ITelegramBotClient client, IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToMainMenuCommand request, CancellationToken cancellationToken)
    {
        await _client.SendReplyKeyboard(request.ChatId, BotMenus.MainMenu);
        await _mediator.Send(new SetBotUserStateRequest(request.ChatId,
            ScreenIds.MainMenu), cancellationToken);

        return Unit.Value;
    }
}