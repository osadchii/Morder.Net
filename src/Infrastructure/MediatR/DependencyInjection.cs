using Infrastructure.MediatR.Companies.Handlers;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
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
    }
}