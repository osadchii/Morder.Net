using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class SaveOrderStickerFromStringRequest : IRequest<Unit>
{
    public int OrderId { get; set; }
    public string Content { get; set; }
    public byte[] Bytes { get; set; }
    public string FileName { get; set; }
}