using System.Net;
using Infrastructure.MediatR.Orders.Company.Queries;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class GetOrderStickerHandler : IRequestHandler<GetOrderStickerRequest, OrderSticker>
{
    private readonly MContext _context;
    private readonly ILogger<GetOrderStickerHandler> _logger;

    public GetOrderStickerHandler(MContext context, ILogger<GetOrderStickerHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OrderSticker> Handle(GetOrderStickerRequest request, CancellationToken cancellationToken)
    {
        var orderData = await _context.Orders
            .AsNoTracking()
            .Where(o => o.ExternalId == request.ExternalId)
            .Select(o => new { o.Id, o.Number, o.ExternalId })
            .SingleOrDefaultAsync(cancellationToken);

        if (orderData is null || orderData.Id == 0)
        {
            throw new HttpRequestException($"Order with {request.ExternalId} external id not found", null,
                HttpStatusCode.NotFound);
        }

        OrderSticker? stickerData = await _context.OrderStickers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.OrderId == orderData.Id, cancellationToken);

        if (stickerData is null)
        {
            throw new HttpRequestException($"Sticker for order {request.ExternalId} not found");
        }

        _logger.LogInformation("Received {OrderNumber} order with {OrderExternalId} external id sticker",
            orderData.Number, orderData.ExternalId);

        return stickerData;
    }
}