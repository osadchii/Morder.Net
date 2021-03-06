using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Prices;

namespace Integration.Common.Services.StocksAndPrices.Prices;

public abstract class MarketplaceSendPriceService
{
    protected readonly IMapper Mapper;
    protected readonly IServiceProvider ServiceProvider;

    protected MarketplaceSendPriceService(IMapper mapper, IServiceProvider serviceProvider)
    {
        Mapper = mapper;
        ServiceProvider = serviceProvider;
    }

    public abstract Task SendPricesAsync(Marketplace marketplace, MarketplacePriceDto[] prices);
}