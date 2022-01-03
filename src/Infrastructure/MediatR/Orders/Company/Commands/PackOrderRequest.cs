using System.ComponentModel.DataAnnotations;
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
}