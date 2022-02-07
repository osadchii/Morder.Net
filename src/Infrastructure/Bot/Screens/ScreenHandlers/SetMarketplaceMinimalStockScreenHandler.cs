using Infrastructure.Bot.MediatR.Commands.Marketplaces;
using Infrastructure.Bot.MediatR.Marketplaces.Commands;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class SetMarketplaceMinimalStockScreenHandler : ScreenHandler
{
    public SetMarketplaceMinimalStockScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        if (!int.TryParse(User.CurrentStateKey, out int marketplaceId))
        {
            return;
        }

        if (Text.Equals(MenuTexts.Back, StringComparison.InvariantCultureIgnoreCase))
        {
            await Mediator.Send(new ToMarketplaceManagementCommand()
            {
                ChatId = ChatId,
                MarketplaceId = marketplaceId
            });
            return;
        }

        if (!decimal.TryParse(Text, out decimal stock))
        {
            return;
        }

        await Mediator.Send(new SetMarketplaceMinimalStockRequest()
        {
            ChatId = ChatId,
            MarketplaceId = marketplaceId,
            MinimalStock = stock
        });
    }
}