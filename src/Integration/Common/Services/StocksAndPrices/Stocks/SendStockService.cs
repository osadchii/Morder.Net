using AutoMapper;
using Infrastructure;
using Infrastructure.MediatR.ChangeTracking.Stocks.Commands;
using Infrastructure.MediatR.Stocks.Queries;
using Infrastructure.Models.Marketplaces;
using Integration.Ozon.Services;
using Integration.SberMegaMarket.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services.StocksAndPrices.Stocks;

public interface ISendStockService
{
    Task SendMarketplaceStocks();
}

public class SendStockService : ISendStockService
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly MContext _context;
    private readonly ILogger<SendStockService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SendStockService(IMediator mediator, MContext context, ILogger<SendStockService> logger,
        IServiceProvider serviceProvider, IMapper mapper)
    {
        _mediator = mediator;
        _context = context;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public async Task SendMarketplaceStocks()
    {
        try
        {
            var marketplaces = await _context.Marketplaces
                .AsNoTracking()
                .Where(m => m.IsActive && m.StockChangesTracking)
                .ToListAsync();

            foreach (var marketplace in marketplaces)
            {
                try
                {
                    var stocks = (await _mediator.Send(new GetMarketplaceStocksRequest
                    {
                        MarketplaceId = marketplace.Id,
                        Limit = marketplace.StockSendLimit
                    })).ToArray();

                    if (!stocks.Any())
                    {
                        continue;
                    }

                    MarketplaceSendStockService sendService = marketplace.Type switch
                    {
                        MarketplaceType.SberMegaMarket => new SberMegaMarketSendStockService(_mediator, _mapper,
                            _serviceProvider),
                        MarketplaceType.Ozon => new OzonSendStockService(_mediator, _mapper, _serviceProvider),
                        _ => null
                    };

                    if (sendService is not null)
                    {
                        await sendService.SendStocksAsync(marketplace, stocks);
                    }

                    await _mediator.Send(new DeleteStockChangesRequest(marketplace.Id,
                        stocks.Select(s => s.ProductId).ToList()));
                    _logger.LogInformation($"Sent {stocks.Length} stocks to {marketplace.Name}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error while sending stocks to {marketplace.Name}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending stocks");
        }
    }
}