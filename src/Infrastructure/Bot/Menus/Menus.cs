using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Bot.Menus;

public static class BotMenus
{
    public static ReplyKeyboardMarkup MainMenu => new KeyboardBuilder(true)
        .AddLine()
        .AddButton(MenuTexts.SummaryYesterday)
        .AddButton(MenuTexts.SummaryToday)
        .AddLine()
        .AddButton(MenuTexts.ToReports)
        .AddButton("Настройки")
        .Build();

    public static ReplyKeyboardMarkup ReportMenu => new KeyboardBuilder()
        .AddLine()
        .AddButton(MenuTexts.ReportOne)
        .AddButton(MenuTexts.ReportTwo)
        .Build();
}