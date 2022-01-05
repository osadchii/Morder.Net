using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Interfaces;

public interface IMessageRouter
{
    Task RouteMessageAsync(ITelegramBotClient bot, Message message);
}