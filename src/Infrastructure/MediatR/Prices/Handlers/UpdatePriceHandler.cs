using Infrastructure.Cache.Interfaces;
using Infrastructure.MediatR.Prices.Commands;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Prices.Handlers;

public class UpdatePriceHandler : IRequestHandler<UpdatePriceRequest, Unit>
{
    private readonly MContext _context;
    private readonly ILogger<UpdatePriceHandler> _logger;
    private readonly IIdExtractor<Product> _productIdExtractor;
    private readonly IIdExtractor<PriceType> _priceTypeIdExtractor;
    private readonly IChangeTrackingService _changeTrackingService;

    public UpdatePriceHandler(MContext context, ILogger<UpdatePriceHandler> logger,
        IIdExtractor<Product> productIdExtractor, IIdExtractor<PriceType> priceTypeIdExtractor,
        IChangeTrackingService changeTrackingService)
    {
        _context = context;
        _logger = logger;
        _productIdExtractor = productIdExtractor;
        _priceTypeIdExtractor = priceTypeIdExtractor;
        _changeTrackingService = changeTrackingService;
    }

    public async Task<Unit> Handle(UpdatePriceRequest request, CancellationToken cancellationToken)
    {
        if (!request.PriceTypeExternalId.HasValue || !request.ProductExternalId.HasValue)
        {
            return Unit.Value;
        }

        int? priceTypeId = await _priceTypeIdExtractor.GetIdAsync(request.PriceTypeExternalId.Value);

        if (priceTypeId is null)
        {
            string message = $"Price type with external id {request.PriceTypeExternalId} not found";
            throw new ArgumentException(message);
        }

        int? productId = await _productIdExtractor.GetIdAsync(request.ProductExternalId.Value);

        if (productId is null)
        {
            string message = $"Product with external id {request.ProductExternalId} not found";
            throw new ArgumentException(message);
        }

        if (!request.Value.HasValue)
        {
            string message = $"Empty price for product {productId.Value} at price type {priceTypeId.Value}";
            throw new ArgumentException(message);
        }

        await _changeTrackingService.TrackPriceChange(productId.Value, cancellationToken);
        await _changeTrackingService.TrackStockChange(productId.Value, cancellationToken);

        Price? dbEntry = await _context.Prices
            .SingleOrDefaultAsync(s => s.ProductId == productId.Value && s.PriceTypeId == priceTypeId.Value,
                cancellationToken);

        if (dbEntry is null)
        {
            return await CreatePrice(priceTypeId.Value,
                productId.Value, request.Value.Value, cancellationToken);
        }

        if (dbEntry.Value == request.Value.Value)
        {
            return Unit.Value;
        }

        return await UpdatePrice(dbEntry, request.Value.Value, cancellationToken);
    }

    private async Task<Unit> CreatePrice(int priceTypeId, int productId, decimal value,
        CancellationToken cancellationToken)
    {
        var stock = new Price
        {
            ProductId = productId,
            PriceTypeId = priceTypeId,
            Value = value
        };

        await _context.AddAsync(stock, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created price for product {productId} at price type {priceTypeId} to {value}");

        return Unit.Value;
    }

    private async Task<Unit> UpdatePrice(Price dbEntry, decimal value,
        CancellationToken cancellationToken)
    {
        dbEntry.Value = value;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            $"Updated stock for product {dbEntry.ProductId} at price type {dbEntry.PriceTypeId} to {value}");

        return Unit.Value;
    }
}