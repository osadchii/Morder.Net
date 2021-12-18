using Infrastructure.Models.Marketplaces.SberMegaMarket;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;

public class RemoveSberMegaMarketRequest : IRequest<SberMegaMarketDto>
{
    public int Id { get; set; }
}