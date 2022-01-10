using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Warehouses;
using Integration.Common.Services.Stocks;
using Integration.Ozon.Clients.Stocks;
using Integration.Ozon.Clients.Stocks.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services;

public class OzonSendStockService : MarketplaceSendStockService
{
    public OzonSendStockService(IMediator mediator, IMapper mapper, IServiceProvider serviceProvider) : base(mediator,
        mapper, serviceProvider)
    {
    }

    public override async Task SendStocksAsync(Marketplace marketplace, IEnumerable<MarketplaceStockDto> stocks)
    {
        var client = ServiceProvider.GetRequiredService<IOzonStockClient>();
        var ozon = Mapper.Map<OzonDto>(marketplace);
        var request = new OzonStockRequest
        {
            Stocks = stocks.Select(s =>
            {
                if (!int.TryParse(s.ProductExternalId, out int productId))
                {
                    throw new Exception($"Wrong ozon external id: {s.ProductExternalId} for product {s.ProductId}");
                }

                return new OzonStock
                {
                    Stock = s.Value,
                    ProductId = productId
                };
            })
        };

        await client.SendStocks(ozon, request);
    }
}