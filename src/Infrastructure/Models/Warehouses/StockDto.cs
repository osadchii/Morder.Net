namespace Infrastructure.Models.Warehouses;

public record StockDto
{
    public string ProductId { get; set; } = null!;
    public decimal Value { get; set; }
}