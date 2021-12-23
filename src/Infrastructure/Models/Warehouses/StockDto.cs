namespace Infrastructure.Models.Warehouses;

public record StockDto
{
    public string ProductId { get; set; }
    public decimal Value { get; set; }
}