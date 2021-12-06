using System;
using Infrastructure.MediatR.PriceTypes.Commands;

namespace Api.Integration.PriceTypes;

public static class TestCases
{
    public static UpdatePriceTypeRequest UpdatePriceTypeRequest => new()
    {
        Name = "Test price type",
        ExternalId = Guid.Parse("2acd3154-5213-4712-9ab2-3f993687ec5c")
    };
}