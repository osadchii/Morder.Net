using System;
using Infrastructure.MediatR.Products.Commands;
using Infrastructure.Models.Products;

namespace Api.Integration.Products;

public static class TestCases
{
    public static UpdateProductRequest UpdateProductRequest => new()
    {
        Name = "Test product",
        Articul = "TestArticul",
        ExternalId = Guid.Parse("ede24db4-cbb0-4f8e-b7cd-c7b6bd406348"),
        CategoryId = Categories.TestCases.UpdateChildCategoryRequest.ExternalId,
        DeletionMark = false,
        Length = 10,
        Width = 11,
        Height = 12,
        Weight = 13,
        Brand = "Test brand",
        Vendor = "Test vendor",
        VendorCode = "TestVendorCode",
        Barcode = "123456789012",
        Vat = Vat.Vat_20
    };
}