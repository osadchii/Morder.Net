using Infrastructure.Bot.MediatR.Commands.Reports;
using Infrastructure.Bot.Menus;
using Infrastructure.Extensions;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class SoldProductsByMarketplaceScreenHandler : ScreenHandler
{
    public SoldProductsByMarketplaceScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        if (Text.Equals(MenuTexts.Back, StringComparison.InvariantCultureIgnoreCase))
        {
            await Mediator.Send(new ToSoldProductCommand()
            {
                ChatId = ChatId
            });
            return;
        }

        int.TryParse(User.CurrentStateKey, out var marketplaceId);

        var startToday = DateTime.Today;
        var endToday = startToday.AddDays(1).AddMilliseconds(-1);

        (DateTime from, DateTime to) dates = Text switch
        {
            MenuTexts.Yesterday => (startToday.AddDays(-1), endToday.AddDays(-1)),
            MenuTexts.Today => (startToday, endToday),
            _ => (new DateTime(), new DateTime())
        };

        await Mediator.Send(new SoldProductsReportRequest()
        {
            From = dates.from.ToUtcTime(),
            To = dates.to.ToUtcTime(),
            ChatId = ChatId,
            MarketplaceId = marketplaceId == 0 ? null : marketplaceId
        });
    }
}