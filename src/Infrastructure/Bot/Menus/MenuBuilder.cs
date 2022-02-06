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
        List<KeyboardButton> lastLine = _buttonLines.Last();
        lastLine.Add(new KeyboardButton(text));
        return this;
    }

    public KeyboardBuilder AddMonthsButtons(DateTime startDate)
    {
        var months = new List<DateTime>();
        DateTime currentDate = startDate;

        while (currentDate <= DateTime.UtcNow)
        {
            months.Add(currentDate);
            currentDate = currentDate.AddMonths(1);
        }

        var count = 0;

        foreach (DateTime month in months.OrderByDescending(m => m))
        {
            if (count > 0 && count % 2 == 0)
            {
                AddLine();
            }

            AddButton(month.ToRussianMonthYearString());

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