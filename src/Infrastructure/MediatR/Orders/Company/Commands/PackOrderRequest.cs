using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class PackOrderRequest : IRequest<Unit>
{
    [Required] public Guid? ExternalId { get; set; }

    [Required] public IEnumerable<PackOrderItem> Items { get; set; }
}

public class PackOrderItem
{
    [Required] public Guid? ProductExternalId { get; set; }

    [Required] public decimal? Count { get; set; }

    [Required]
    [Range(Limits.OrderBoxMinimalNumber, Limits.OrderBoxMaximalNumber)]
    public int? Number { get; set; }
}