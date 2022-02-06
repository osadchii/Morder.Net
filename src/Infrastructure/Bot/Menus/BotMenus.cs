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
        .AddButton(MenuTexts.ToMarketplaces)
        .AddLine()
        .AddButton(MenuTexts.ToUsers)
        .Build();

    public static ReplyKeyboardMarkup ReportMenu => new KeyboardBuilder()
        .AddLine()
        .AddButton(MenuTexts.OrdersCount)
        .AddButton(MenuTexts.OrdersSum)
        .AddLine()
        .AddButton(MenuTexts.RatingBrand)
        .AddButton(MenuTexts.RatingProduct)
        .AddLine()
        .AddButton(MenuTexts.SoldProducts)
        .Build();
}