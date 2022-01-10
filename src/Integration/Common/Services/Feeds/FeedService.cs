using System.Diagnostics;
using AutoMapper;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Models.Marketplaces;
using Integration.Meso.Feeds;
using Integration.SberMegaMarket.Feeds;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.Feeds;

public interface IFeedService
{
    Task GenerateFeeds();
}

public class FeedService : IFeedService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly ILogger<FeedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public FeedService(IMediator mediator, IMapper mapper, MContext context, ILogger<FeedService> logger,
        IServiceProvider serviceProvider)
    {
        _mediator = mediator;
        _mapper = mapper;
        _context = context;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task GenerateFeeds()
    {
        try
        {
            List<Marketplace> marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive && MarketplaceConstants.MarketplacesHasFeed.Contains(m.Type))
                .ToListAsync();

            foreach (Marketplace marketplace in marketplaces)
            {
                MarketplaceFeedService? feedService = marketplace.Type switch
                {
                    MarketplaceType.SberMegaMarket => new SberMegaMarketFeedService(_mapper, _serviceProvider,
                        marketplace),
                    MarketplaceType.Meso => new MesoFeedService(_mapper, _serviceProvider, marketplace),
                    _ => null
                };

                if (feedService is null)
                {
                    continue;
                }

                var sw = new Stopwatch();
                sw.Start();

                await feedService.GenerateFeed();

                sw.Stop();
                _logger.LogInformation(
                    $"Generated {marketplace.Name} ({marketplace.Id}) feed. Elapsed {sw.ElapsedMilliseconds} ms");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while generating feeds");
        }
    }
}