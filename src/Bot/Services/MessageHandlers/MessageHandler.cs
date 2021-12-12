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
    protected long ChatId => Message.Chat.Id;
    protected string Text => Message.Text!;

    public MessageHandler(ITelegramBotClient client, IMediator mediator, Message message, BotUser user)
    {
        Client = client;
        Mediator = mediator;
        Message = message;
        User = user;
    }

    public virtual async Task Handle()
    {
        await Client.SendReplyKeyboard(ChatId, BotMenus.MainMenu);
        await SetUserState(StateIds.MainMenu);
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

    public static async Task<bool> IsToMainMenu(
        ITelegramBotClient client, IMediator mediator, Message message, BotUser user)
    {
        if (message.Text == MenuTexts.ToMainMenu)
        {
            await client.SendReplyKeyboard(message.Chat.Id, BotMenus.MainMenu);
            await SetUserState(mediator, user, StateIds.MainMenu);

            return true;
        }

        return false;
    }
}