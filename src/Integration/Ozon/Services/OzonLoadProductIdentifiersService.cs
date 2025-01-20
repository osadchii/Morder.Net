using AutoMapper;
using Infrastructure.MediatR.ProductIdentifiers.Commands;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Infrastructure.Models.Products;
using Infrastructure.Services.Marketplaces;
using Integration.Common.Services.Products;
using Integration.Ozon.Clients.LoadProducts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Integration.Ozon.Services;

public class OzonLoadProductIdentifiersService : MarketplaceLoadProductIdentifiersService
{
    public OzonLoadProductIdentifiersService(IMapper mapper, IServiceProvider serviceProvider) : base(mapper,
        serviceProvider)
    {
    }

    public override async Task LoadProductIdentifiersAsync(Marketplace marketplace)
    {
        var client = ServiceProvider.GetRequiredService<IOzonLoadProductIdentifiersClient>();
        var articulService = ServiceProvider.GetRequiredService<IProductArticulService>();
        var logger = ServiceProvider.GetRequiredService<ILogger<OzonLoadProductIdentifiersService>>();
        var mediator = ServiceProvider.GetRequiredService<IMediator>();

        var ozon = Mapper.Map<OzonDto>(marketplace);

        if (!ozon.Settings.LoadProductIds)
        {
            return;
        }

        var result = await client.LoadOzonProductIdentifiersAsync(ozon);

        foreach (var kv in result)
        {
            var productId = await articulService.GetProductIdByArticul(kv.Key);

            if (productId == 0)
            {
                logger.LogWarning("Product with {OfferId} not found", kv.Key);
                continue;
            }

            if (!string.IsNullOrEmpty(kv.Value.FboSku))
            {
                await mediator.Send(new SetProductIdentifierRequest
                {
                    Type = ProductIdentifierType.OzonFbo,
                    MarketplaceId = ozon.Id,
                    ProductId = productId,
                    Value = kv.Value.FboSku
                });
            }

            if (!string.IsNullOrEmpty(kv.Value.FbsSku))
            {
                await mediator.Send(new SetProductIdentifierRequest
                {
                    Type = ProductIdentifierType.OzonFbs,
                    MarketplaceId = ozon.Id,
                    ProductId = productId,
                    Value = kv.Value.FbsSku
                });
            }

            if (!string.IsNullOrEmpty(kv.Value.OzonId))
            {
                await mediator.Send(new SetProductIdentifierRequest
                {
                    Type = ProductIdentifierType.StockAndPrice,
                    MarketplaceId = ozon.Id,
                    ProductId = productId,
                    Value = kv.Value.OzonId
                });
            }
        }
    }
}