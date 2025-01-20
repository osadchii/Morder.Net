using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Marketplaces.Kuper;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Kuper.Commands;

public class UpdateKuperRequest : BaseUpdateMarketplaceRequest, IRequest<KuperDto>
{
    [Required] public KuperSettings Settings { get; set; }
}