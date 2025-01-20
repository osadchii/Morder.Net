using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Bot.MediatR.Commands.Reports;
using Infrastructure.Bot.Menus;
using Infrastructure.Extensions;
using Infrastructure.Models.BotUsers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class OrdersSumScreenHandler : ScreenHandler
{
    public OrdersSumScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
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

        try
        {
            var dates = Text.FromRussianInterval();
            await Mediator.Send(new OrdersSumReportRequest()
            {
                From = dates.from.ToUtcTime(),
                To = dates.to.ToUtcTime(),
                ChatId = ChatId
            });
        }
        catch (Exception ex)
        {
            var logger = ServiceProvider.GetRequiredService<ILogger<OrdersSumScreenHandler>>();
            logger.LogError(ex, "Can't parse dates from {Message}", Text);
        }
    }
}