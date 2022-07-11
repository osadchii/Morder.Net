using AutoMapper;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Models.Marketplaces;
using Integration.Ozon.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.Products;

public interface ILoadProductIdentifiersService
{
    Task LoadMarketplaceProductIds();
}

public class LoadProductIdentifiersService : ILoadProductIdentifiersService
{
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly ILogger<LoadProductIdentifiersService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public LoadProductIdentifiersService(MContext context, ILogger<LoadProductIdentifiersService> logger,
        IServiceProvider serviceProvider, IMapper mapper)
    {
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
                try
                {
                    MarketplaceLoadProductIdentifiersService sendService = marketplace.Type switch
                    {
                        MarketplaceType.Ozon => new OzonLoadProductIdentifiersService(_mapper, _serviceProvider),
                        _ => null
                    };

                    if (sendService is null)
                    {
                        continue;
                    }

                    await sendService.LoadProductIdentifiersAsync(marketplace);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while loading product identifiers from {MarketplaceName}", 
                        marketplace.Name);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading product ids");
        }
    }
}