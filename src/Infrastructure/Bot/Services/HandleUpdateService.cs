using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Bot.Services;

public class HandleUpdateService
{
    private readonly ILogger<HandleUpdateService> _logger;
    private readonly IMessageRouter _router;

    public HandleUpdateService(ILogger<HandleUpdateService> logger, IMessageRouter router)
    {
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
            HandleError(exception);
        }
    }

    private Task BotOnMessageReceived(Message message)
    {
        if (message.Type != MessageType.Text)
            return Task.CompletedTask;

        return _router.Route(message);
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    private void HandleError(Exception exception)
    {
        string errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogError(exception, "HandleError: {ErrorMessage}", errorMessage);
    }
}