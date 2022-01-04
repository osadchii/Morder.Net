using System.Text;
using Infrastructure.MediatR.Orders.Company.Commands;
using Infrastructure.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.Orders.Company.Handlers;

public class SaveOrderStickerFromStringHandler : IRequestHandler<SaveOrderStickerFromStringRequest, Unit>
{
    private readonly MContext _context;

    public SaveOrderStickerFromStringHandler(MContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SaveOrderStickerFromStringRequest request, CancellationToken cancellationToken)
    {
        byte[] data = Encoding.UTF8.GetBytes(request.Content);

        OrderSticker? dbEntry = await _context.OrderStickers
            .FirstOrDefaultAsync(s => s.OrderId == request.OrderId, cancellationToken);

        if (dbEntry is null)
        {
            await _context.OrderStickers.AddAsync(new OrderSticker()
            {
                Name = request.FileName,
                OrderId = request.OrderId,
                StickerData = data
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        dbEntry.StickerData = data;
        dbEntry.Name = request.FileName;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}