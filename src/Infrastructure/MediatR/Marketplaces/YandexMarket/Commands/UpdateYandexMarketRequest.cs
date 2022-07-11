using System.ComponentModel.DataAnnotations;
using Infrastructure.Models.Marketplaces.YandexMarket;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.YandexMarket.Commands;

public class UpdateYandexMarketRequest : BaseUpdateMarketplaceRequest, IRequest<YandexMarketDto>
{
    [Required] public YandexMarketSettings Settings { get; set; }
}