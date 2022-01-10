using Infrastructure.MediatR.Marketplaces.Meso.Commands;
using Infrastructure.Models.Marketplaces.Meso;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Meso.Handlers;

public class UpdateMesoHandler : IRequestHandler<UpdateMesoRequest, MesoDto>
{
    private readonly IMarketplaceUpdateService<UpdateMesoRequest, MesoDto> _updateService;

    public UpdateMesoHandler(
        IMarketplaceUpdateService<UpdateMesoRequest, MesoDto> updateService)
    {
        _updateService = updateService;
    }

    public Task<MesoDto> Handle(UpdateMesoRequest request, CancellationToken cancellationToken)
    {
        return _updateService.UpdateMarketplaceAsync(request, cancellationToken);
    }
}