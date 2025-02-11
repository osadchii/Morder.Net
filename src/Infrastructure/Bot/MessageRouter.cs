using Infrastructure.Bot.MediatR.Commands.Common;
using Infrastructure.Bot.MediatR.Commands.Orders.Commands;
using Infrastructure.Bot.MediatR.Commands.Users;
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
    Task HandlerCallbackQuery(CallbackQuery callbackQuery);
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
        var user = await GetUser(message);

        await _mediator.Send(new IncrementBotUserUsageCounterRequest
        {
            BotUserId = user.Id
        });

        var hasAccess = await _mediator.Send(new UserAccessCheckRequest
        {
            User = user
        });

        if (!hasAccess)
        {
            return;
        }

        if ((message.Text ?? string.Empty) == MenuTexts.ToMainMenu)
        {
            await _mediator.Send(new ToMainMenuCommand
            {
                ChatId = message.Chat.Id
            });
            return;
        }

        ScreenHandler handler = user.CurrentState switch
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
            ScreenIds.SoldProducts => new SoldProductsScreenHandler(_serviceProvider, message, user),
            ScreenIds.SoldProductsByMarketplace => new SoldProductsByMarketplaceScreenHandler(_serviceProvider, message,
                user),
            ScreenIds.UsersMenu => new UsersScreenHandler(_serviceProvider, message, user),
            ScreenIds.UserManagement => new UserManagementScreenHandler(_serviceProvider, message, user),
            ScreenIds.MarketplacesMenu => new MarketplacesScreenHandler(_serviceProvider, message, user),
            ScreenIds.MarketplaceManagement => new MarketplaceManagementScreenHandler(_serviceProvider, message, user),
            ScreenIds.SetMinimalPrice => new SetMarketplaceMinimalPriceScreenHandler(_serviceProvider, message, user),
            ScreenIds.SetMinimalStock => new SetMarketplaceMinimalStockScreenHandler(_serviceProvider, message, user),
            _ => null
        };

        if (handler is null)
        {
            await _mediator.Send(new ToMainMenuCommand
            {
                ChatId = message.Chat.Id
            });

            return;
        }

        await handler.HandleMessage();
    }

    public async Task HandlerCallbackQuery(CallbackQuery callbackQuery)
    {
        var from = callbackQuery.From;
        var user = await GetUser(from.Id, from.FirstName ?? string.Empty, from.LastName ?? string.Empty,
            from.Username ?? string.Empty);

        await _mediator.Send(new HandlerCallbackQueryRequest
        {
            BotUserId = user.Id,
            Data = callbackQuery.Data
        });
    }

    private Task<BotUser> GetUser(Message message)
    {
        var chat = message.Chat;
        return GetUser(chat.Id, chat.FirstName ?? string.Empty, chat.LastName ?? string.Empty,
            chat.Username ?? string.Empty);
    }

    private Task<BotUser> GetUser(long chatId, string firstName, string lastName, string userName)
    {
        return _mediator.Send(new CreateUpdateBotUserRequest
        {
            ChatId = chatId,
            FirstName = firstName,
            LastName = lastName,
            UserName = userName
        });
    }
}