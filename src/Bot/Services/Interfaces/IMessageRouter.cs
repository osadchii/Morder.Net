using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Services.Interfaces;

public interface IMessageRouter
{
    Task RouteMessageAsync(ITelegramBotClient bot, Message message);
}