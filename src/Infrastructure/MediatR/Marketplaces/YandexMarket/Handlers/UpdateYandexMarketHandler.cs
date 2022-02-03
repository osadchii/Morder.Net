using Infrastructure.MediatR.Marketplaces.YandexMarket.Commands;
using Infrastructure.Models.Marketplaces.YandexMarket;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.YandexMarket.Handlers;

public class UpdateYandexMarketHandler : IRequestHandler<UpdateYandexMarketRequest, YandexMarketDto>
{
    private readonly IMarketplaceUpdateService<UpdateYandexMarketRequest, YandexMarketDto> _updateService;

    public UpdateYandexMarketHandler(
        IMarketplaceUpdateService<UpdateYandexMarketRequest, YandexMarketDto> updateService)
    {
        _updateService = updateService;
    }

    public Task<YandexMarketDto> Handle(UpdateYandexMarketRequest request, CancellationToken cancellationToken)
    {
        return _updateService.UpdateMarketplaceAsync(request, cancellationToken);
    }
}