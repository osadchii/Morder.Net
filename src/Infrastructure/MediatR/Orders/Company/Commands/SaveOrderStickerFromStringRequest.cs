using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class SaveOrderStickerFromStringRequest : IRequest<Unit>
{
    public int OrderId { get; set; }
    public string Content { get; set; } = null!;
    public byte[] Bytes { get; set; } = null!;
    public string FileName { get; set; } = null!;
}