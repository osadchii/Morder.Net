using AutoMapper;
using Infrastructure;
using Infrastructure.Marketplaces;
using Infrastructure.MediatR.ChangeTracking.Commands;
using Infrastructure.MediatR.Prices.Queries;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Prices;
using Integration.SberMegaMarket.Stocks.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Integration.Common.Services;

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
                MarketplacePriceDto[] stocks = (await _mediator.Send(new GetMarketplacePricesRequest()
                {
                    MarketplaceId = marketplace.Id,
                    Limit = marketplace.PriceSendLimit
                })).ToArray();

                if (!stocks.Any())
                {
                    continue;
                }

                MarketplaceSendPriceService? sendService = marketplace.Type switch
                {
                    MarketplaceType.SberMegaMarket => new SberMegaMarketSendPriceService(_mediator, _mapper,
                        _serviceProvider),
                    _ => null
                };

                if (sendService is not null)
                {
                    await sendService.SendPricesAsync(marketplace, stocks);
                }

                await _mediator.Send(new DeletePriceChangesRequest(marketplace.Id,
                    stocks.Select(s => s.ProductId).ToList()));
                _logger.LogInformation($"Sent {stocks.Length} prices to {marketplace.Name}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending prices");
        }
    }
}