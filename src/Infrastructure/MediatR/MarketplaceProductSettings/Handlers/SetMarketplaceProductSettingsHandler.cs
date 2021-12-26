using AutoMapper;
using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using Infrastructure.Models.MarketplaceProductSettings;
using Infrastructure.Services.Marketplaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.MediatR.MarketplaceProductSettings.Handlers;

public class SetMarketplaceProductSettingsHandler : IRequestHandler<SetMarketplaceProductSettingsRequest, Unit>
{
    private readonly MContext _context;
    private readonly IMapper _mapper;
    private readonly IChangeTrackingService _changeTrackingService;

    public SetMarketplaceProductSettingsHandler(MContext context, IMapper mapper,
        IChangeTrackingService changeTrackingService)
    {
        _context = context;
        _mapper = mapper;
        _changeTrackingService = changeTrackingService;
    }

    public async Task<Unit> Handle(SetMarketplaceProductSettingsRequest request, CancellationToken cancellationToken)
    {
        MarketplaceProductSetting? dbEntry = await _context.MarketplaceProductSettings
            .SingleOrDefaultAsync(s => s.MarketplaceId == request.MarketplaceId
                                       && s.ProductId == request.ProductId, cancellationToken: cancellationToken);

        await _changeTrackingService.TrackStockChange(request.MarketplaceId, request.ProductId, cancellationToken);


        if (dbEntry is null)
        {
            return await CreateSetting(request, cancellationToken);
        }

        return await UpdateSetting(dbEntry, request, cancellationToken);
    }

    private async Task<Unit> CreateSetting(SetMarketplaceProductSettingsRequest request,
        CancellationToken cancellationToken)
    {
        var dbEntry = _mapper.Map<MarketplaceProductSetting>(request);

        await _context.AddAsync(dbEntry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<Unit> UpdateSetting(MarketplaceProductSetting dbEntry,
        SetMarketplaceProductSettingsRequest request,
        CancellationToken cancellationToken)
    {
        _mapper.Map(request, dbEntry);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}