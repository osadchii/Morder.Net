using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Bot.MediatR.Commands.Users;
using Infrastructure.Bot.MediatR.Orders.Queries;
using Infrastructure.Bot.Menus;
using Infrastructure.Extensions;
using Infrastructure.Models.BotUsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class MainMenuScreenHandler : ScreenHandler
{
    private readonly string _botOwnerUserName;

    public MainMenuScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        _botOwnerUserName = configuration.GetSection("BotConfiguration")["BotOwnerUserName"];
    }

    public override async Task HandleMessage()
    {
        switch (Text)
        {
            case MenuTexts.TodaySummary:
                await Mediator.Send(new GetOrderSummaryByDayRequest()
                {
                    ChatId = ChatId,
                    Date = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).ToUtcTime()
                });
                break;
            case MenuTexts.YesterdaySummary:
                await Mediator.Send(new GetOrderSummaryByDayRequest()
                {
                    ChatId = ChatId,
                    Date = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(-1)
                        .ToUtcTime()
                });
                break;
            case MenuTexts.ToReports:
                await Mediator.Send(new ToReportMenuCommand()
                {
                    ChatId = ChatId
                });
                break;
            case MenuTexts.ToUsers:
                if (!User.Administrator && User.UserName != _botOwnerUserName)
                {
                    await Client.SendTextAsync(ChatId, MessageConstants.OnlyAdministratorsMessage);
                    break;
                }

                await Mediator.Send(new ToUsersCommand()
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