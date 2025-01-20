using Infrastructure.MediatR.Marketplaces.Kuper.Commands;
using Infrastructure.Models.Marketplaces.Kuper;
using Infrastructure.Services.Marketplaces;
using MediatR;

namespace Infrastructure.MediatR.Marketplaces.Kuper.Handlers;

public class UpdateKuperHandler : IRequestHandler<UpdateKuperRequest, KuperDto>
{
    private readonly IMarketplaceUpdateService<UpdateKuperRequest, KuperDto> _updateService;

    public UpdateKuperHandler(
        IMarketplaceUpdateService<UpdateKuperRequest, KuperDto> updateService)
    {
        _updateService = updateService;
    }

    public Task<KuperDto> Handle(UpdateKuperRequest request, CancellationToken cancellationToken)
    {
        return _updateService.UpdateMarketplaceAsync(request, cancellationToken);
    }
}