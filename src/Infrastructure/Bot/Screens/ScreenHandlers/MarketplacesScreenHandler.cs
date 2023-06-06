using Infrastructure.Bot.MediatR.Commands.Marketplaces;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class MarketplacesScreenHandler : ScreenHandler
{
    public MarketplacesScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        var splitText = Text.Split('–');
        var firstPart = splitText[0].Trim();

        if (int.TryParse(firstPart, out var marketplaceId))
        {
            await Mediator.Send(new ToMarketplaceManagementCommand()
            {
                ChatId = ChatId,
                MarketplaceId = marketplaceId
            });
        }
    }
}