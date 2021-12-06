using Infrastructure.MediatR.Stocks.Commands;

namespace Api.Integration.Stocks;

public static class TestCases
{
    public static UpdateStockRequest UpdateStockRequest => new()
    {
        ProductExternalId = Products.TestCases.UpdateProductRequest.ExternalId,
        WarehouseExternalId = Warehouses.TestCases.UpdateWarehouseRequest.ExternalId,
        Value = 10
    };
}