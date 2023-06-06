using System.Globalization;
using System.Text;
using Infrastructure.Bot.Menus;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Bot;

public static class TelegramExtensions
{
    public static async Task SendTextAsync(this ITelegramBotClient client, long chatId, string text)
    {
        const int maxMessageLength = 4096;

        var lines = text.Split(Environment.NewLine);
        var textToSend = new StringBuilder();

        foreach (var line in lines)
        {
            if (textToSend.Length + line.Length > maxMessageLength)
            {
                await client.SendTextMessageAsync(chatId, textToSend.ToString(), ParseMode.Html);
                textToSend.Clear();
            }

            textToSend.AppendLine(line);
        }

        if (textToSend.Length > 0)
        {
            await client.SendTextMessageAsync(chatId, textToSend.ToString(), ParseMode.Html);
        }
    }


    public static async Task SendReplyKeyboard(this ITelegramBotClient client, long chatId, IReplyMarkup keyboard)
    {
        await client.SendTextMessageAsync(chatId: chatId,
            text: MessageConstants.SelectMenuItem,
            replyMarkup: keyboard);
    }

    public static string ToFormatString(this decimal value)
    {
        var cultureInfo = CultureInfo.GetCultureInfo("es-ES");
        return Math.Round(value).ToString("##,#", cultureInfo);
    }

    public static DateTime ToUtcWithMoscowOffset(this DateTime date)
    {
        TimeZoneInfo moscow = TimeZoneInfo.FindSystemTimeZoneById
            ("Russian Standard Time");

        TimeSpan offset = moscow.GetUtcOffset(date);
        return date - offset;
    }

    public static string ToRussianMonthYearString(this DateTime date)
    {
        DateTimeFormatInfo info = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        var monthName = info.MonthNames[date.Month - 1];

        return $"{monthName} {date.Year}";
    }

    public static (DateTime from, DateTime to) FromRussianInterval(this string dateString)
    {
        if (dateString.Equals(MenuTexts.Days7, StringComparison.InvariantCultureIgnoreCase))
        {
            return (DateTime.Now.AddDays(-7), DateTime.Now);
        }

        if (dateString.Equals(MenuTexts.Days28, StringComparison.InvariantCultureIgnoreCase))
        {
            return (DateTime.Now.AddDays(-28), DateTime.Now);
        }

        if (dateString.Equals(MenuTexts.Days90, StringComparison.InvariantCultureIgnoreCase))
        {
            return (DateTime.Now.AddDays(-90), DateTime.Now);
        }

        DateTimeFormatInfo info = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        var splitDate = dateString.Split(' ');

        if (splitDate.Length != 2)
        {
            throw new Exception("Wrong date format");
        }

        if (!int.TryParse(splitDate[1], out var year))
        {
            throw new Exception("Wrong date format");
        }

        for (var i = 0; i < info.MonthNames.Length; i++)
        {
            if (info.MonthNames[i].Equals(splitDate[0], StringComparison.InvariantCultureIgnoreCase))
            {
                var from = new DateTime(year, i + 1, 1);
                DateTime to = from.AddMonths(1).AddMilliseconds(-1);
                return (from, to);
            }
        }

        throw new Exception("Wrong date format");
    }
}