using Infrastructure.MediatR.Marketplaces.Ozon.Commands;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Ozon.Handlers;

public class UpdateOzonHandler : IRequestHandler<UpdateOzonRequest, OzonDto>
{
    private readonly IMarketplaceUpdateService<UpdateOzonRequest, OzonDto> _updateService;

    public UpdateOzonHandler(
        IMarketplaceUpdateService<UpdateOzonRequest, OzonDto> updateService)
    {
        _updateService = updateService;
    }

    public Task<OzonDto> Handle(UpdateOzonRequest request, CancellationToken cancellationToken)
    {
        return _updateService.UpdateMarketplaceAsync(request, cancellationToken);
    }
}