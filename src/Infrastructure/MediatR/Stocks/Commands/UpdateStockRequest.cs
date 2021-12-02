using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Stocks.Commands;

public class UpdateStockRequest : IRequest<Unit>
{
    [Required] public Guid? ProductExternalId { get; set; }

    [Required] public Guid? WarehouseExternalId { get; set; }

    [Required]
    [Range(Limits.MinimalStock, Limits.MaximalStock)]
    public decimal? Value { get; set; }
}