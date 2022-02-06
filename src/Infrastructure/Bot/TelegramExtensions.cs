using System.Globalization;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Bot;

public static class TelegramExtensions
{
    public static async Task SendTextAsync(this ITelegramBotClient client, long chatId, string text)
    {
        const int maxMessageLength = 4096;

        string[] lines = text.Split(Environment.NewLine);
        var textToSend = new StringBuilder();

        foreach (string line in lines)
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

    public static string ToRussianMonthYearString(this DateTime date)
    {
        DateTimeFormatInfo info = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        string monthName = info.MonthNames[date.Month - 1];

        return $"{monthName} {date.Year}";
    }

    public static DateTime FromRussianMonthYearString(this string dateString)
    {
        DateTimeFormatInfo info = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
        string[] splitDate = dateString.Split(' ');

        if (splitDate.Length != 2)
        {
            throw new Exception("Wrong date format");
        }

        if (!int.TryParse(splitDate[1], out int year))
        {
            throw new Exception("Wrong date format");
        }

        for (var i = 0; i < info.MonthNames.Length; i++)
        {
            if (info.MonthNames[i].Equals(splitDate[0], StringComparison.InvariantCultureIgnoreCase))
            {
                return new DateTime(year, i + 1, 1);
            }
        }

        throw new Exception("Wrong date format");
    }
}