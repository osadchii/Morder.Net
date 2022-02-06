using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using MediatR;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Common;

public class ToReportMenuCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
}

public class ToReportMenuHandler : IRequestHandler<ToReportMenuCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly IMediator _mediator;

    public ToReportMenuHandler(ITelegramBotClient client, IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToReportMenuCommand request, CancellationToken cancellationToken)
    {
        await _client.SendReplyKeyboard(request.ChatId, BotMenus.ReportMenu);
        await _mediator.Send(new SetBotUserStateRequest(request.ChatId,
            ScreenIds.ReportMenu), cancellationToken);

        return Unit.Value;
    }
}