using Infrastructure.Bot.MediatR.Commands.Marketplaces;
using Infrastructure.Bot.MediatR.Marketplaces.Commands;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class SetMarketplaceMinimalPriceScreenHandler : ScreenHandler
{
    public SetMarketplaceMinimalPriceScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
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

        if (!decimal.TryParse(Text, out decimal price))
        {
            return;
        }

        await Mediator.Send(new SetMarketplaceMinimalPriceRequest()
        {
            ChatId = ChatId,
            MarketplaceId = marketplaceId,
            MinimalPrice = price
        });
    }
}