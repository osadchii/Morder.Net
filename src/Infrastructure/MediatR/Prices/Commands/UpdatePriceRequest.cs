using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.Prices.Commands;

public class UpdatePriceRequest : IRequest<Unit>
{
    [Required] public Guid? ProductExternalId { get; set; }

    [Required] public Guid? PriceTypeExternalId { get; set; }

    [Required]
    [Range(Limits.MinimalPrice, Limits.MaximalPrice)]
    public decimal? Value { get; set; }
}