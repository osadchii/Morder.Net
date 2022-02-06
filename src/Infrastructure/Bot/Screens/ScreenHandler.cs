using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens;

public abstract class ScreenHandler
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly Message Message;
    protected readonly BotUser User;

    protected ScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
    {
        ServiceProvider = serviceProvider;
        Message = message;
        User = user;
    }

    protected long ChatId => Message.Chat.Id;
    protected string Text => Message.Text ?? string.Empty;

    protected IMediator Mediator => ServiceProvider.GetRequiredService<IMediator>();
    protected MContext Context => ServiceProvider.GetRequiredService<MContext>();
    protected ITelegramBotClient Client => ServiceProvider.GetRequiredService<ITelegramBotClient>();

    public abstract Task HandleMessage();
}