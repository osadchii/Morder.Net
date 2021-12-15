using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;

public class UpdateSberMegaMarketRequest : BaseUpdateMarketplaceRequest, IRequest<SberMegaMarketDto>
{
    [Required] public SberMegaMarketSettings? Settings { get; set; }
}