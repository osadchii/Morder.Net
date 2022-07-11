using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Marketplaces.Meso;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Meso.Commands;

public class UpdateMesoRequest : BaseUpdateMarketplaceRequest, IRequest<MesoDto>
{
    [Required] public MesoSettings Settings { get; set; }
}