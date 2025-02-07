using Infrastructure.Bot.MediatR.Commands.Reports;
using Infrastructure.Bot.Menus;
using Infrastructure.Extensions;
using Infrastructure.Models.BotUsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class BrandRatingByMarketplaceScreenHandler : ScreenHandler
{
    public BrandRatingByMarketplaceScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        if (Text.Equals(MenuTexts.Back, StringComparison.InvariantCultureIgnoreCase))
        {
            await Mediator.Send(new ToBrandRatingCommand
            {
                ChatId = ChatId
            });
            return;
        }

        int.TryParse(User.CurrentStateKey, out var marketplaceId);

        try
        {
            var dates = Text.FromRussianInterval();
            await Mediator.Send(new BrandRatingReportRequest
            {
                From = dates.from.ToUtcTime(),
                To = dates.to.ToUtcTime(),
                ChatId = ChatId,
                MarketplaceId = marketplaceId == 0 ? null : marketplaceId
            });
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider.GetRequiredService<ILogger<BrandRatingByMarketplaceScreenHandler>>();
            logger.LogError(ex, "Can't parse dates from {Message}", Text);
        }
    }
}