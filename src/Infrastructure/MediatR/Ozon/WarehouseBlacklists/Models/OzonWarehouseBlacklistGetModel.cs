using Infrastructure.Common;

namespace Infrastructure.MediatR.Ozon.WarehouseBlacklists.Models;

public class OzonWarehouseBlacklistGetModel
{
    public IdNameModel<int>[] Products { get; set; }
}