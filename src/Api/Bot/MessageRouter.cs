using Bot.States;
using Infrastructure.Bot;
using Infrastructure.Bot.Interfaces;
using Infrastructure.Bot.MessageHandlers;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Api.Bot;

public class MessageRouter : IMessageRouter
{
    private readonly IMediator _mediator;
    private readonly ILogger<MessageRouter> _logger;
    private readonly string _ownerUserName;
    private BotUser? _user;

    public MessageRouter(IMediator mediator, ILogger<MessageRouter> logger,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _logger = logger;

        var config = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        _ownerUserName = config.BotOwnerUserName!;
    }

    public async Task RouteMessageAsync(ITelegramBotClient bot, Message message)
    {
        await SetCurrentUser(message);

        if (_user is null || _user.UserName != _ownerUserName && !_user.Verified)
        {
            await AccessDenied(bot, message);
            return;
        }

        bool toMainMenu = await MessageHandler.IsToMainMenu(bot, _mediator, message, _user);

        if (toMainMenu)
        {
            return;
        }

        MessageHandler handler = _user.CurrentState switch
        {
            StateIds.MainMenu => new MainMenuHandler(bot, _mediator, message, _user, _logger),
            _ => new MessageHandler(bot, _mediator, message, _user, _logger)
        };

        await handler.Handle();
    }

    private async Task SetCurrentUser(Message message)
    {
        _user = await _mediator.Send(new CreateUpdateBotUserRequest
        {
            ChatId = message.Chat.Id,
            FirstName = message.Chat.FirstName,
            LastName = message.Chat.LastName,
            UserName = message.Chat.Username
        });
    }

    private static async Task AccessDenied(ITelegramBotClient bot, Message message)
    {
        await bot.SendTextAsync(chatId: message.Chat.Id,
            text: MessageConstants.AccessDeniedMessage);
    }
}