using Infrastructure.Bot.MediatR.Commands.Marketplaces;
using Infrastructure.Bot.MediatR.Marketplaces.Commands;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class MarketplaceManagementScreenHandler : ScreenHandler
{
    public MarketplaceManagementScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        if (Text.Equals(MenuTexts.Back, StringComparison.InvariantCultureIgnoreCase))
        {
            await Mediator.Send(new ToMarketplacesCommand()
            {
                ChatId = ChatId
            });
            return;
        }

        if (!int.TryParse(User.CurrentStateKey, out var marketplaceId))
        {
            return;
        }

        switch (Text)
        {
            case MenuTexts.SetMinimalPrice:
                await Mediator.Send(new ToSetMinimalPriceCommand()
                {
                    ChatId = ChatId,
                    MarketplaceId = marketplaceId
                });
                break;
            case MenuTexts.SetMinimalStock:
                await Mediator.Send(new ToSetMinimalStockCommand()
                {
                    ChatId = ChatId,
                    MarketplaceId = marketplaceId
                });
                break;
            case MenuTexts.NullifyStockOn:
                await Mediator.Send(new SetMarketplaceNullifyStocksRequest()
                {
                    ChatId = ChatId,
                    MarketplaceId = marketplaceId,
                    NullifyStocks = true
                });
                break;
            case MenuTexts.NullifyStockOff:
                await Mediator.Send(new SetMarketplaceNullifyStocksRequest()
                {
                    ChatId = ChatId,
                    MarketplaceId = marketplaceId,
                    NullifyStocks = false
                });
                break;
        }
    }
}