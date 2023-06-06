using Infrastructure.Common;
using Infrastructure.MediatR.Ozon.WarehouseBlacklists.Models;
using Infrastructure.Models.Marketplaces.Ozon;

namespace Infrastructure.MediatR.Ozon.WarehouseBlacklists.Mappings;

public static class OzonWarehouseBlacklistMappingExtensions
{
    public static OzonWarehouseBlacklistGetModel Map(this IEnumerable<OzonWarehouseBlacklist> blacklist)
    {
        return new OzonWarehouseBlacklistGetModel
        {
            Products = blacklist
                .Select(x => new IdNameModel<int>
                {
                    Id = x.ProductId,
                    Name = x.Product.Name
                })
                .ToArray()
        };
    }
}