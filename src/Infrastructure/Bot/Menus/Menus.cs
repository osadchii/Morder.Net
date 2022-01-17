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
        .AddLine()
        .AddButton(MenuTexts.ToMarketplaces)
        .Build();

    public static ReplyKeyboardMarkup ReportMenu => new KeyboardBuilder()
        .AddLine()
        .AddButton(MenuTexts.ReportOne)
        .AddButton(MenuTexts.ReportTwo)
        .Build();

    public static ReplyKeyboardMarkup MarketplaceMenu => new KeyboardBuilder()
        .AddLine()
        .AddButton(MenuTexts.BackButton)
        .AddLine()
        .AddButton(MenuTexts.MarketplaceChangeMinimalPrice)
        .AddButton(MenuTexts.MarketplaceChangeMinimalStock)
        .Build();
}