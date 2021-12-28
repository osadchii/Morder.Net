using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Marketplaces.Ozon;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Ozon.Commands;

public class UpdateOzonRequest : BaseUpdateMarketplaceRequest, IRequest<OzonDto>
{
    [Required] public OzonSettings? Settings { get; set; }
}