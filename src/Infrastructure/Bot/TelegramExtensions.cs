using System.Text;
using Telegram.Bot;
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
                await client.SendTextMessageAsync(chatId, textToSend.ToString());
                textToSend.Clear();
            }

            textToSend.AppendLine(line);
        }

        if (textToSend.Length > 0)
        {
            await client.SendTextMessageAsync(chatId, textToSend.ToString());
        }
    }


    public static async Task SendReplyKeyboard(this ITelegramBotClient client, long chatId, IReplyMarkup keyboard)
    {
        await client.SendTextMessageAsync(chatId: chatId,
            text: MessageConstants.SelectMenuItem,
            replyMarkup: keyboard);
    }
}