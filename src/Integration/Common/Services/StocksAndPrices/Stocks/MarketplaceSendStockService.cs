using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Warehouses;
using MediatR;

namespace Integration.Common.Services.StocksAndPrices.Stocks;

public abstract class MarketplaceSendStockService
{
    protected readonly IMediator Mediator;
    protected readonly IMapper Mapper;
    protected readonly IServiceProvider ServiceProvider;

    protected MarketplaceSendStockService(IMediator mediator, IMapper mapper, IServiceProvider serviceProvider)
    {
        Mediator = mediator;
        Mapper = mapper;
        ServiceProvider = serviceProvider;
    }

    public abstract Task SendStocksAsync(Marketplace marketplace, MarketplaceStockDto[] stocks);
}