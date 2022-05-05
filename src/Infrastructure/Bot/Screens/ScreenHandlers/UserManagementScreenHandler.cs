using Infrastructure.Bot.MediatR.Commands.Users;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.Screens.ScreenHandlers;

public class UserManagementScreenHandler : ScreenHandler
{
    public UserManagementScreenHandler(IServiceProvider serviceProvider, Message message, BotUser user)
        : base(serviceProvider, message, user)
    {
    }

    public override async Task HandleMessage()
    {
        if (Text.Equals(MenuTexts.Back, StringComparison.InvariantCultureIgnoreCase))
        {
            await Mediator.Send(new ToUsersCommand()
            {
                ChatId = ChatId
            });
            return;
        }

        if (!int.TryParse(User.CurrentStateKey, out int userId))
        {
            return;
        }

        switch (Text)
        {
            case MenuTexts.Verify:
                await Mediator.Send(new SetUserVerifyRequest()
                {
                    ChatId = ChatId,
                    Verified = true,
                    UserId = userId
                });
                break;
            case MenuTexts.Block:
                await Mediator.Send(new SetUserVerifyRequest()
                {
                    ChatId = ChatId,
                    Verified = false,
                    UserId = userId
                });
                break;
            case MenuTexts.AddAdministrator:
                await Mediator.Send(new SetUserAdministratorRequest()
                {
                    ChatId = ChatId,
                    Administrator = true,
                    UserId = userId
                });
                break;
            case MenuTexts.RemoveAdministrator:
                await Mediator.Send(new SetUserAdministratorRequest()
                {
                    ChatId = ChatId,
                    Administrator = false,
                    UserId = userId
                });
                break;
            case MenuTexts.RemoveUser:
                await Mediator.Send(new RemoveUserRequest()
                {
                    ChatId = ChatId,
                    UserId = userId
                });
                break;
        }
    }
}