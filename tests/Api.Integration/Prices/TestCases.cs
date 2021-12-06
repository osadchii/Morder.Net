using Infrastructure.MediatR.Prices.Commands;

namespace Api.Integration.Prices;

public static class TestCases
{
    public static UpdatePriceRequest UpdatePriceRequest => new()
    {
        ProductExternalId = Products.TestCases.UpdateProductRequest.ExternalId,
        PriceTypeExternalId = PriceTypes.TestCases.UpdatePriceTypeRequest.ExternalId,
        Value = 10
    };
}