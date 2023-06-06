using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Bot.Menus;

public class KeyboardBuilder
{
    private readonly List<List<KeyboardButton>> _buttonLines;
    private readonly bool _isMainMenu;

    public KeyboardBuilder(bool isMainMenu = false)
    {
        _buttonLines = new List<List<KeyboardButton>>();
        _isMainMenu = isMainMenu;
    }

    public KeyboardBuilder AddLine()
    {
        _buttonLines.Add(new List<KeyboardButton>());
        if (!_isMainMenu && _buttonLines.Count == 1)
        {
            AddButton(MenuTexts.ToMainMenu);
            AddLine();
        }

        return this;
    }

    public KeyboardBuilder AddButton(string text)
    {
        var lastLine = _buttonLines.Last();
        lastLine.Add(new KeyboardButton(text));
        return this;
    }

    public KeyboardBuilder AddDateIntervalButtons(DateTime startDate)
    {
        const int buttonsPerLine = 3;

        var months = new List<DateTime>();
        var currentDate = new DateTime(startDate.Year, startDate.Month, 1);

        while (currentDate <= DateTime.UtcNow)
        {
            months.Add(currentDate);
            currentDate = currentDate.AddMonths(1);
        }

        var names = new List<string>()
        {
            MenuTexts.Days7,
            MenuTexts.Days28,
            MenuTexts.Days90
        };
        names.AddRange(
            months
                .OrderByDescending(m => m)
                .Select(month => month.ToRussianMonthYearString()));

        var count = 0;

        foreach (var name in names)
        {
            if (count > 0 && count % buttonsPerLine == 0)
            {
                AddLine();
            }

            AddButton(name);
            count++;
        }

        return this;
    }

    public ReplyKeyboardMarkup Build()
    {
        var result = new ReplyKeyboardMarkup(_buttonLines);
        result.ResizeKeyboard = true;

        return result;
    }
}