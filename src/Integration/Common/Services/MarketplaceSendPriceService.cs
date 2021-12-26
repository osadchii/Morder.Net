using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Prices;
using MediatR;

namespace Integration.Common.Services;

public abstract class MarketplaceSendPriceService
{
    protected readonly IMediator Mediator;
    protected readonly IMapper Mapper;
    protected readonly IServiceProvider ServiceProvider;

    protected MarketplaceSendPriceService(IMediator mediator, IMapper mapper, IServiceProvider serviceProvider)
    {
        Mediator = mediator;
        Mapper = mapper;
        ServiceProvider = serviceProvider;
    }

    public abstract Task SendPricesAsync(Marketplace marketplace, IEnumerable<MarketplacePriceDto> stocks);
}