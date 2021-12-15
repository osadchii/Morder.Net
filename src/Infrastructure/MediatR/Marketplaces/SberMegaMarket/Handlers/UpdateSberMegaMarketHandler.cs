using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.SberMegaMarket.Handlers;

public class UpdateSberMegaMarketHandler : IRequestHandler<UpdateSberMegaMarketRequest, SberMegaMarketDto>
{
    private readonly IMarketplaceUpdateService<UpdateSberMegaMarketRequest, SberMegaMarketDto> _updateService;

    public UpdateSberMegaMarketHandler(
        IMarketplaceUpdateService<UpdateSberMegaMarketRequest, SberMegaMarketDto> updateService)
    {
        _updateService = updateService;
    }

    public Task<SberMegaMarketDto> Handle(UpdateSberMegaMarketRequest request, CancellationToken cancellationToken)
    {
        return _updateService.UpdateMarketplaceAsync(request, cancellationToken);
    }
}