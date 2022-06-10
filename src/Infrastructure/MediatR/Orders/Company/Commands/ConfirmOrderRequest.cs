using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.MediatR.Orders.Company.Commands;

public class ConfirmOrderRequest : IRequest<Unit>
{
    [Required] public Guid? ExternalId { get; set; }
    public string User { get; set; } = null!;
}