using AutoMapper;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Marketplaces;
using Infrastructure.MediatR.MarketplaceProductSettings.Commands;
using Infrastructure.Models.Marketplaces;
using Integration.Ozon.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.Products;

public interface ILoadProductIdService
{
    Task LoadMarketplaceProductIds();
}

public class LoadProductIdService : ILoadProductIdService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly ILogger<LoadProductIdService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoadProductIdService(IMediator mediator, MContext context, ILogger<LoadProductIdService> logger,
        IServiceProvider serviceProvider, IMapper mapper)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public async Task LoadMarketplaceProductIds()
    {
        try
        {
            List<Marketplace> marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive && !MarketplaceConstants.MarketplacesHasNoExternalProductId.Contains(m.Type))
                .ToListAsync();

            foreach (Marketplace marketplace in marketplaces)
            {
                MarketplaceLoadProductIdsService? sendService = marketplace.Type switch
                {
                    MarketplaceType.Ozon => new OzonLoadProductIdsService(_mapper, _serviceProvider),
                    _ => null
                };

                if (sendService is null)
                {
                    continue;
                }

                Dictionary<string, string> result = await sendService.LoadProductIds(marketplace);

                if (result.Count == 0)
                {
                    continue;
                }

                await _mediator.Send(new SetMarketplaceProductExternalIdsRequest
                {
                    ExternalIds = result,
                    MarketplaceId = marketplace.Id
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading product ids");
        }
    }
}