using System.Collections.Generic;
using Infrastructure.MediatR.Marketplaces.SberMegaMarket.Commands;
using Infrastructure.Models.Marketplaces.SberMegaMarket;
using Infrastructure.Models.Products;

namespace Api.Integration.Marketplaces.SberMegaMarket;

public static class TestCases
{
    public static UpdateSberMegaMarketRequest UpdateSberMegaMarketRequest => new UpdateSberMegaMarketRequest()
    {
        Id = 1,
        Name = "Test Sber",
        Settings = new SberMegaMarketSettings()
        {
            Token = "Test token"
        },
        WarehouseExternalId = Warehouses.TestCases.UpdateWarehouseRequest.ExternalId,
        IsActive = true,
        MinimalPrice = 100,
        NullifyStocks = false,
        ProductTypes = new List<ProductType>()
        {
            ProductType.Piece,
            ProductType.Weight
        },
        PriceTypeExternalId = PriceTypes.TestCases.UpdatePriceTypeRequest.ExternalId
    };
}