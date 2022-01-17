using System.Text;
using Infrastructure.Bot.MediatR.Marketplaces.Queries;
using Infrastructure.Models.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Bot.MediatR.Marketplaces.Handlers;

public class GetMarketplaceInformationHandler : IRequestHandler<GetMarketplaceInformationRequest, string>
{
    private readonly MContext _context;

    public GetMarketplaceInformationHandler(MContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetMarketplaceInformationRequest request, CancellationToken cancellationToken)
    {
        Marketplace marketplace = await _context.Marketplaces.AsNoTracking()
            .SingleAsync(m => m.Id == request.MarketplaceId, cancellationToken);

        var builder = new StringBuilder();
        builder.AppendLine($"Идентификатор: {marketplace.Id}");
        builder.AppendLine($"Название: {marketplace.Name}");
        builder.AppendLine($"Тип: {marketplace.Type}");
        builder.AppendLine($"Активен: {(marketplace.IsActive ? "Да" : "Нет")}");
        builder.AppendLine($"Обнулены остатки: {(marketplace.NullifyStocks ? "Да" : "Нет")}");
        builder.AppendLine($"Минимальная цена: {marketplace.MinimalPrice}");
        builder.AppendLine($"Минимальный остаток: {marketplace.MinimalStock}");

        return builder.ToString();
    }
}