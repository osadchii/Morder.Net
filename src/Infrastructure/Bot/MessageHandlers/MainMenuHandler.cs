using Bot.States;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.MessageHandlers;

public class MainMenuHandler : MessageHandler
{
    public override Task Handle()
    {
        return Text switch
        {
            MenuTexts.SummaryToday => SendSummary(DateOnly.FromDateTime(DateTime.Today)),
            MenuTexts.SummaryYesterday => SendSummary(DateOnly.FromDateTime(DateTime.Today).AddDays(-1)),
            MenuTexts.ToReports => ToReports(),
            _ => Task.CompletedTask
        };
    }

    public MainMenuHandler(ITelegramBotClient client, IMediator mediator, Message message, BotUser user, ILogger logger)
        : base(client, mediator, message, user, logger)
    {
    }

    private async Task ToReports()
    {
        await Client.SendReplyKeyboard(ChatId, BotMenus.ReportMenu);
        await SetUserState(StateIds.Reports);
        LogRouting("Reports");
    }

    private Task SendSummary(DateOnly date)
    {
        return Client.SendTextAsync(ChatId, $"Здесь будет сводка за {date}");
    }
}