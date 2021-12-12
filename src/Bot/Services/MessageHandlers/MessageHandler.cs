using Bot.Extensions;
using Bot.Menus;
using Bot.States;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Bot.Services.MessageHandlers;

public class MessageHandler
{
    protected readonly IMediator Mediator;
    protected readonly ITelegramBotClient Client;
    protected readonly Message Message;
    protected readonly BotUser User;
    protected readonly ILogger Logger;
    protected long ChatId => Message.Chat.Id;
    protected string Text => Message.Text!;

    public MessageHandler(ITelegramBotClient client, IMediator mediator, Message message, BotUser user, ILogger logger)
    {
        Client = client;
        Mediator = mediator;
        Message = message;
        User = user;
        Logger = logger;
    }

    public virtual async Task Handle()
    {
        await ToMainMenu(Client, Mediator, Message, User);
        LogRouting("Default route");
    }

    protected Task SetUserState(string state, string? key = null)
    {
        return SetUserState(Mediator, User, state, key);
    }

    private static async Task SetUserState(IMediator mediator, BotUser user, string state, string? key = null)
    {
        var request = new SetBotUserStateRequest(user.ChatId, state, key);
        await mediator.Send(request);
    }

    private static async Task ToMainMenu(ITelegramBotClient client, IMediator mediator, Message message, BotUser user)
    {
        await client.SendReplyKeyboard(message.Chat.Id, BotMenus.MainMenu);
        await SetUserState(mediator, user, StateIds.MainMenu);
    }

    protected void LogRouting(string routedTo)
    {
        Logger.LogInformation(
            $@"User: {User}" +
            $"\nText: {Text}" +
            $"\nState: {User.CurrentState}" +
            $"\nState key: {User.CurrentStateKey}" +
            $"\nRouted to: {routedTo}");
    }

    public static async Task<bool> IsToMainMenu(
        ITelegramBotClient client, IMediator mediator, Message message, BotUser user)
    {
        if (message.Text == MenuTexts.ToMainMenu)
        {
            await ToMainMenu(client, mediator, message, user);

            return true;
        }

        return false;
    }
}