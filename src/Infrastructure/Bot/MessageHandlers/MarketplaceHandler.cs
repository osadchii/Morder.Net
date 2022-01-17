using Bot.States;
using Infrastructure.Bot.MediatR.Marketplaces.Commands;
using Infrastructure.Bot.MediatR.Marketplaces.Queries;
using Infrastructure.Bot.Menus;
using Infrastructure.Models.BotUsers;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Infrastructure.Bot.MessageHandlers;

public class MarketplaceHandler : MessageHandler
{
    private readonly int? _marketplaceId;

    public MarketplaceHandler(ITelegramBotClient client, IMediator mediator, Message message, BotUser user,
        ILogger logger, MContext context) : base(client, mediator, message, user, logger, context)
    {
        if (int.TryParse(User.CurrentStateKey, out int marketplaceId))
        {
            _marketplaceId = marketplaceId;
        }
    }

    public override Task Handle()
    {
        return User.CurrentState switch
        {
            StateIds.Marketplaces => HandleMarketplaceState(),
            StateIds.MarketplaceSetMinimalPrice => HandleSetMinimalPriceState(),
            StateIds.MarketplaceSetMinimapStock => HandleSetMinimalStockState(),
            _ => Task.CompletedTask
        };
    }

    private async Task HandleSetMinimalPriceState()
    {
        if (!_marketplaceId.HasValue)
        {
            return;
        }

        if (!decimal.TryParse(Text, out decimal minimalPrice))
        {
            await Client.SendTextAsync(ChatId, "Некорректное значение");
            return;
        }

        await Mediator.Send(new SetMarketplaceMinimalPriceRequest()
        {
            MarketplaceId = _marketplaceId.Value,
            MinimalPrice = minimalPrice,
            UserChatId = User.ChatId
        });
        await SendMarketplaceMenu(_marketplaceId.Value);
    }

    private Task HandleSetMinimalStockState()
    {
        return Task.CompletedTask;
    }

    private Task HandleMarketplaceState()
    {
        if (!_marketplaceId.HasValue)
        {
            string[] messageParts = Text.Split("–");

            if (messageParts.Length > 1 && int.TryParse(messageParts[0], out int marketplaceId))
            {
                return SendMarketplaceMenu(marketplaceId);
            }
        }

        return Text switch
        {
            MenuTexts.BackButton => ToMarketplaces(),
            MenuTexts.MarketplaceChangeMinimalPrice => SetMarketplaceMinimalPrice(),
            MenuTexts.MarketplaceChangeMinimalStock => Task.CompletedTask,
            _ => Task.CompletedTask
        };
    }

    private async Task SetMarketplaceMinimalPrice()
    {
        if (!_marketplaceId.HasValue)
        {
            return;
        }

        await Client.SendTextAsync(ChatId, "Введите минимальную цену");
        await SetUserState(StateIds.MarketplaceSetMinimalPrice, _marketplaceId.Value.ToString());
    }

    private async Task SendMarketplaceMenu(int marketplaceId)
    {
        await Client.SendTextAsync(ChatId, await Mediator.Send(new GetMarketplaceInformationRequest()
        {
            MarketplaceId = marketplaceId
        }));
        await Client.SendReplyKeyboard(ChatId, BotMenus.MarketplaceMenu);
        await SetUserState(StateIds.Marketplaces, marketplaceId.ToString());
        LogRouting("Marketplaces");
    }
}