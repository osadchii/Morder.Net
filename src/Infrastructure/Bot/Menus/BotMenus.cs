using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Bot.Menus;

public static class BotMenus
{
    public static ReplyKeyboardMarkup MainMenu => new KeyboardBuilder(true)
        .AddLine()
        .AddButton(MenuTexts.YesterdaySummary)
        .AddButton(MenuTexts.TodaySummary)
        .AddLine()
        .AddButton(MenuTexts.ToReports)
        .AddLine()
        .AddButton(MenuTexts.ToMarketplaces)
        .Build();
}