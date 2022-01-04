using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class ShipOrderRequest : IRequest<Unit>
{
    [Required] public Guid? ExternalId { get; set; }
}