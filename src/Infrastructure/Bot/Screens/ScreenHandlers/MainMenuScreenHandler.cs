using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Bot.MediatR.Orders.Queries;
using Infrastructure.Bot.Menus;
using Infrastructure.Extensions;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class MainMenuScreenHandler : ScreenHandler
{
    public MainMenuScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        switch (Text)
        {
            case MenuTexts.TodaySummary:
                await Mediator.Send(new GetOrderSummaryByDayRequest()
                {
                    ChatId = ChatId,
                    Date = DateTime.Today.ToUtcTime()
                });
                break;
            case MenuTexts.YesterdaySummary:
                await Mediator.Send(new GetOrderSummaryByDayRequest()
                {
                    ChatId = ChatId,
                    Date = DateTime.Today.AddDays(-1).ToUtcTime()
                });
                break;
            case MenuTexts.ToReports:
                await Mediator.Send(new ToReportMenuCommand()
                {
                    ChatId = ChatId
                });
                break;
            default:
                await Mediator.Send(new ToMainMenuCommand()
                {
                    ChatId = ChatId
                });
                break;
        }
    }
}