using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Warehouses.Commands;

public class UpdateWarehouseRequest : IRequest<Unit>
{
    [Required]
    [MaxLength(Limits.WarehouseName)]
    public string? Name { get; set; }

    [Required] public Guid? ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}