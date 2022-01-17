using Bot.States;
using Infrastructure.Bot.Menus;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.MessageHandlers;

public class MessageHandler
{
    protected readonly IMediator Mediator;
    protected readonly ITelegramBotClient Client;
    protected readonly Message Message;
    protected readonly BotUser User;
    protected readonly ILogger Logger;
    protected readonly MContext Context;
    protected long ChatId => Message.Chat.Id;
    protected string Text => Message.Text!;

    public MessageHandler(ITelegramBotClient client, IMediator mediator, Message message, BotUser user, ILogger logger,
        MContext context)
    {
        Client = client;
        Mediator = mediator;
        Message = message;
        User = user;
        Logger = logger;
        Context = context;
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

    protected async Task ToMarketplaces()
    {
        List<Marketplace> marketplaces = await Context.Marketplaces
            .AsNoTracking()
            .OrderBy(m => m.Id)
            .ToListAsync();

        var menuBuilder = new KeyboardBuilder();

        foreach (Marketplace marketplace in marketplaces)
        {
            menuBuilder.AddLine()
                .AddButton($"{marketplace.Id} – {marketplace.Name} ({marketplace.Type})");
        }

        await Client.SendReplyKeyboard(ChatId, menuBuilder.Build());
        await SetUserState(StateIds.Marketplaces);
        LogRouting("Marketplaces");
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