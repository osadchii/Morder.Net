using System;
using Infrastructure.MediatR.Warehouses.Commands;

namespace Api.Integration.Warehouses;

public static class TestCases
{
    public static UpdateWarehouseRequest UpdateWarehouseRequest => new()
    {
        Name = "Test warehouse",
        ExternalId = Guid.Parse("67db5582-c117-4c35-9c8b-1aa961a74737")
    };
}