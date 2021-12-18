using Infrastructure.Models.Marketplaces.SberMegaMarket;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.SberMegaMarket.Queries;

public class GetAllActiveSberMegaMarketSettingsRequest : IRequest<List<SberMegaMarketDto>>
{
}