using System.Text;
using Infrastructure.Bot.Menus;
using Infrastructure.Bot.Screens;
using Infrastructure.MediatR.BotUsers.Commands;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

namespace Infrastructure.Bot.MediatR.Commands.Marketplaces;

public class ToMarketplaceManagementCommand : IRequest<Unit>
{
    public long ChatId { get; set; }
    public int MarketplaceId { get; set; }
}

public class ToMarketplaceManagementHandler : IRequestHandler<ToMarketplaceManagementCommand, Unit>
{
    private readonly ITelegramBotClient _client;
    private readonly MContext _context;
    private readonly IMediator _mediator;

    public ToMarketplaceManagementHandler(ITelegramBotClient client, MContext context, IMediator mediator)
    {
        _client = client;
        _context = context;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(ToMarketplaceManagementCommand request, CancellationToken cancellationToken)
    {
        Marketplace marketplace = await _context.Marketplaces
            .AsNoTracking()
            .SingleAsync(b => b.Id == request.MarketplaceId, cancellationToken);

        var sb = new StringBuilder();
        sb.AppendLine($"Название: {marketplace.Name}");
        sb.AppendLine($"Тип: {marketplace.Type}");
        sb.AppendLine($"Активен: {marketplace.IsActive}");
        sb.AppendLine($"Обнулять остатки: {marketplace.NullifyStocks}");
        sb.AppendLine($"Минимальная цена: {marketplace.MinimalPrice}");
        sb.AppendLine($"Минимальный остаток: {marketplace.MinimalStock}");

        await _client.SendTextAsync(request.ChatId, sb.ToString());

        await _client.SendReplyKeyboard(request.ChatId, new KeyboardBuilder()
            .AddLine()
            .AddButton(MenuTexts.Back)
            .AddLine()
            .AddButton(MenuTexts.SetMinimalPrice)
            .AddLine()
            .AddButton(MenuTexts.SetMinimalStock)
            .AddLine()
            .AddButton(marketplace.NullifyStocks ? MenuTexts.NullifyStockOff : MenuTexts.NullifyStockOn)
            .Build());

        await _mediator.Send(new SetBotUserStateRequest(request.ChatId, ScreenIds.MarketplaceManagement,
            request.MarketplaceId.ToString()), cancellationToken);

        return Unit.Value;
    }
}