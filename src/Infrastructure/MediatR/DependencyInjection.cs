using Infrastructure.MediatR.Companies.Handlers;
using Infrastructure.MediatR.Marketplaces.Meso.Commands;
using Infrastructure.MediatR.Marketplaces.Ozon.Commands;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.MediatR.Marketplaces.YandexMarket.Commands;
using Infrastructure.Models.Marketplaces.Meso;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Marketplaces.YandexMarket;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MediatR;

public static class MediatRDependencyInjection
{
    public static void AddMorderMediatR(this IServiceCollection services)
    {
        services.AddMediatR(typeof(UpdateCompanyInformationHandler).Assembly);
        services.AddTransient<IMarketplaceUpdateService<UpdateSberMegaMarketRequest, SberMegaMarketDto>,
            MarketplaceUpdateService<UpdateSberMegaMarketRequest, SberMegaMarketDto>>();
        services.AddTransient<IMarketplaceUpdateService<UpdateOzonRequest, OzonDto>,
            MarketplaceUpdateService<UpdateOzonRequest, OzonDto>>();
        services.AddTransient<IMarketplaceUpdateService<UpdateMesoRequest, MesoDto>,
            MarketplaceUpdateService<UpdateMesoRequest, MesoDto>>();
        services.AddTransient<IMarketplaceUpdateService<UpdateYandexMarketRequest, YandexMarketDto>,
            MarketplaceUpdateService<UpdateYandexMarketRequest, YandexMarketDto>>();
    }
}