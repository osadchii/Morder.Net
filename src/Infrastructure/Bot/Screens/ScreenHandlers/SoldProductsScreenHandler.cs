using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Bot.MediatR.Commands.Reports;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class SoldProductsScreenHandler : ScreenHandler
{
    public SoldProductsScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        if (Text.Equals(MenuTexts.Back, StringComparison.InvariantCultureIgnoreCase))
        {
            await Mediator.Send(new ToReportMenuCommand()
            {
                ChatId = ChatId
            });
            return;
        }

        if (Text.Equals(MenuTexts.Total, StringComparison.InvariantCultureIgnoreCase))
        {
            await Mediator.Send(new ToSoldProductsByMarketplaceCommand()
            {
                ChatId = ChatId,
                MarketplaceId = null
            });
            return;
        }

        var splitText = Text.Split('–');
        var firstPart = splitText[0].Trim();

        if (int.TryParse(firstPart, out var marketplaceId))
        {
            await Mediator.Send(new ToSoldProductsByMarketplaceCommand()
            {
                ChatId = ChatId,
                MarketplaceId = marketplaceId
            });
        }
    }
}