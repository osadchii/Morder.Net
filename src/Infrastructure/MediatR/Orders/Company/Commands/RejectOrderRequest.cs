using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class RejectOrderRequest : IRequest<Unit>
{
    [Required] public Guid? ExternalId { get; set; }

    [Required] public IEnumerable<RejectOrderItem> Items { get; set; }
    public string User { get; set; } = null!;
}

public class RejectOrderItem
{
    [Required] public Guid? ProductExternalId { get; set; }

    [Required] public decimal? Count { get; set; }
}