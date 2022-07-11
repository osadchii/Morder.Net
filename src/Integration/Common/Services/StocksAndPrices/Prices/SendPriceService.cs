using AutoMapper;
using Infrastructure;
using Infrastructure.MediatR.ChangeTracking.Prices.Commands;
using Infrastructure.MediatR.Prices.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Prices;
using Integration.Ozon.Services;
using Integration.SberMegaMarket.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.StocksAndPrices.Prices;

public interface ISendPriceService
{
    Task SendMarketplacePrices();
}

public class SendPriceService : ISendPriceService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly ILogger<SendPriceService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SendPriceService(IMediator mediator, MContext context, ILogger<SendPriceService> logger,
        IServiceProvider serviceProvider, IMapper mapper)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public async Task SendMarketplacePrices()
    {
        try
        {
            List<Marketplace> marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive && m.PriceChangesTracking)
                .ToListAsync();

            foreach (Marketplace marketplace in marketplaces)
            {
                try
                {
                    MarketplacePriceDto[] prices = (await _mediator.Send(new GetMarketplacePricesRequest()
                    {
                        MarketplaceId = marketplace.Id,
                        Limit = marketplace.PriceSendLimit
                    })).ToArray();

                    if (!prices.Any())
                    {
                        continue;
                    }

                    MarketplaceSendPriceService sendService = marketplace.Type switch
                    {
                        MarketplaceType.SberMegaMarket => new SberMegaMarketSendPriceService(_mapper,
                            _serviceProvider),
                        MarketplaceType.Ozon => new OzonSendPriceService(_mapper, _serviceProvider),
                        _ => null
                    };

                    if (sendService is not null)
                    {
                        await sendService.SendPricesAsync(marketplace, prices);
                    }

                    await _mediator.Send(new DeletePriceChangesRequest(marketplace.Id,
                        prices.Select(s => s.ProductId).ToList()));
                    _logger.LogInformation("Sent {Count} prices to {MarketplaceName}", prices.Length, marketplace.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while sending prices to {MarketplaceName}", marketplace.Name);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending prices");
        }
    }
}