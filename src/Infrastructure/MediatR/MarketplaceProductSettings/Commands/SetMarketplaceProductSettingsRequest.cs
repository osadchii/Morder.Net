using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Infrastructure.MediatR.MarketplaceProductSettings.Commands;

public class SetMarketplaceProductSettingsRequest : IRequest<Unit>
{
    [Required] public int MarketplaceId { get; set; }

    [Required] public int ProductId { get; set; }

    [Required] public bool NullifyStock { get; set; }

    [Required] public bool IgnoreRestrictions { get; set; }

    public string? Externalid { get; set; }
}