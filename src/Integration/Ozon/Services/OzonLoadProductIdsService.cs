using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services.Products;
using Integration.Ozon.Clients.LoadProducts;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Ozon.Services;

public class OzonLoadProductIdsService : MarketplaceLoadProductIdsService
{
    public OzonLoadProductIdsService(IMapper mapper, IServiceProvider serviceProvider) : base(mapper, serviceProvider)
    {
    }

    public override async Task<Dictionary<string, string>> LoadProductIds(Marketplace marketplace)
    {
        var client = ServiceProvider.GetRequiredService<IOzonLoadProductIdsClient>();
        var ozon = Mapper.Map<OzonDto>(marketplace);

        if (!ozon.Settings.LoadProductIds)
        {
            return new Dictionary<string, string>();
        }

        return await client.LoadProductIdsAsync(ozon);
    }
}