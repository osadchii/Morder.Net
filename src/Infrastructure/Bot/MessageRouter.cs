using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.Bot.Screens.ScreenHandlers;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.BotUsers;
using MediatR;
using Telegram.Bot.Types;

namespace Infrastructure.Bot;

public interface IMessageRouter
{
    Task Route(Message message);
}

public class MessageRouter : IMessageRouter
{
    private readonly IMediator _mediator;
    private readonly IServiceProvider _serviceProvider;

    public MessageRouter(IMediator mediator, IServiceProvider serviceProvider)
    {
        _mediator = mediator;
        _serviceProvider = serviceProvider;
    }

    public async Task Route(Message message)
    {
        BotUser user = await GetUser(message);

        bool hasAccess = await _mediator.Send(new UserAccessCheckRequest()
        {
            User = user
        });

        if (!hasAccess)
        {
            return;
        }

        if ((message.Text ?? string.Empty) == MenuTexts.ToMainMenu)
        {
            await _mediator.Send(new ToMainMenuCommand()
            {
                ChatId = message.Chat.Id
            });
            return;
        }

        ScreenHandler? handler = user.CurrentState switch
        {
            ScreenIds.MainMenu => new MainMenuScreenHandler(_serviceProvider, message, user),
            ScreenIds.ReportMenu => new ReportMenuScreenHandler(_serviceProvider, message, user),
            ScreenIds.OrdersSum => new OrdersSumScreenHandler(_serviceProvider, message, user),
            ScreenIds.OrdersCount => new OrdersCountScreenHandler(_serviceProvider, message, user),
            ScreenIds.ProductRating => new ProductRatingScreenHandler(_serviceProvider, message, user),
            ScreenIds.ProductRatingByMarketplace => new ProductRatingByMarketplaceScreenHandler(_serviceProvider,
                message, user),
            ScreenIds.BrandRating => new BrandRatingScreenHandler(_serviceProvider, message, user),
            ScreenIds.BrandRatingByMarketplace => new BrandRatingByMarketplaceScreenHandler(_serviceProvider, message,
                user),
            _ => null
        };

        if (handler is null)
        {
            await _mediator.Send(new ToMainMenuCommand()
            {
                ChatId = message.Chat.Id
            });

            return;
        }

        await handler.HandleMessage();
    }

    private Task<BotUser> GetUser(Message message)
    {
        return _mediator.Send(new CreateUpdateBotUserRequest()
        {
            ChatId = message.Chat.Id,
            FirstName = message.Chat.FirstName ?? string.Empty,
            LastName = message.Chat.LastName ?? string.Empty,
            UserName = message.Chat.Username ?? string.Empty
        });
    }
}