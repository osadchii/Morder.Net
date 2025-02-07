using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Bot.MediatR.Commands.Reports;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class ReportMenuScreenHandler : ScreenHandler
{
    public ReportMenuScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        switch (Text)
        {
            case MenuTexts.OrdersSum:
                await Mediator.Send(new ToOrdersSumCommand
                {
                    ChatId = ChatId
                });
                break;
            case MenuTexts.OrdersCount:
                await Mediator.Send(new ToOrdersCountCommand
                {
                    ChatId = ChatId
                });
                break;
            case MenuTexts.RatingProduct:
                await Mediator.Send(new ToProductRatingCommand
                {
                    ChatId = ChatId
                });
                break;
            case MenuTexts.RatingBrand:
                await Mediator.Send(new ToBrandRatingCommand
                {
                    ChatId = ChatId
                });
                break;
            case MenuTexts.SoldProducts:
                await Mediator.Send(new ToSoldProductCommand
                {
                    ChatId = ChatId
                });
                break;
            default:
                await Mediator.Send(new ToMainMenuCommand
                {
                    ChatId = ChatId
                });
                break;
        }
    }
}