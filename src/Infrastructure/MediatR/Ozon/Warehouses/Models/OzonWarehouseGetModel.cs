namespace Infrastructure.MediatR.Ozon.Warehouses.Models;

public class OzonWarehouseGetModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long OzonWarehouseId { get; set; }
}