using Infrastructure.Bot.MediatR.Commands.Users;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class UsersScreenHandler : ScreenHandler
{
    public UsersScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        var splitText = Text.Split('–');
        var firstPart = splitText[0].Trim();

        if (int.TryParse(firstPart, out var userId))
        {
            await Mediator.Send(new ToUserManagementCommand
            {
                ChatId = ChatId,
                UserId = userId
            });
        }
    }
}