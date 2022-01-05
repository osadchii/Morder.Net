using Infrastructure.Bot.Interfaces;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Bot.Services;

public class HandleUpdateService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HandleUpdateService> _logger;
    private readonly IMessageRouter _router;

    public HandleUpdateService(ITelegramBotClient botClient,
        ILogger<HandleUpdateService> logger, IMessageRouter router)
    {
        _botClient = botClient;
        _logger = logger;
        _router = router;
    }

    public async Task EchoAsync(Update update)
    {
        Task handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message!),
            _ => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(exception);
        }
    }

    private async Task BotOnMessageReceived(Message message)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Type != MessageType.Text)
            return;

        await _router.RouteMessageAsync(_botClient, message);
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    private Task HandleErrorAsync(Exception exception)
    {
        string errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogError(exception, "HandleError: {ErrorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}