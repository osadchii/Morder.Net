using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using MediatR;

namespace Infrastructure.MediatR.PriceTypes.Commands;

public class UpdatePriceTypeRequest : IRequest<Unit>
{
    [Required]
    [MaxLength(Limits.PriceTypeName)]
    public string Name { get; set; } = null!;

    [Required] public Guid? ExternalId { get; set; }

    public bool DeletionMark { get; set; }
}