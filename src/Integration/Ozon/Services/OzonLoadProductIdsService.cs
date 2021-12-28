using AutoMapper;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Marketplaces.Ozon;
using Integration.Common.Services;
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
        var sber = Mapper.Map<OzonDto>(marketplace);

        return await client.LoadProductIdsAsync(sber);
    }
}