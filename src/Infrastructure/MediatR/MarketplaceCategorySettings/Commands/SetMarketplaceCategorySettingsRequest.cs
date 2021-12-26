using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.MediatR.MarketplaceCategorySettings.Commands;

public class SetMarketplaceCategorySettingsRequest : IRequest<Unit>
{
    [Required] public int? MarketplaceId { get; set; }

    [Required] public int? CategoryId { get; set; }

    [Required] public bool? Blocked { get; set; }
}