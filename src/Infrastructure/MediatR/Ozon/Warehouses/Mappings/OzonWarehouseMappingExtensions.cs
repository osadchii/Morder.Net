using Infrastructure.MediatR.Ozon.Warehouses.Models;
using Infrastructure.Models.Marketplaces.Ozon;

namespace Infrastructure.MediatR.Ozon.Warehouses.Mappings;

public static class OzonWarehouseMappingExtensions
{
    public static OzonWarehouseGetModel Map(this OzonWarehouse entity)
    {
        return new OzonWarehouseGetModel
        {
            OzonWarehouseId = entity.OzonWarehouseId,
            Name = entity.Name,
            Id = entity.Id
        };
    }
}