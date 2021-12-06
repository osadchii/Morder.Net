using System;
using Infrastructure.MediatR.Categories.Commands;

namespace Api.Integration.Categories;

public static class TestCases
{
    public static UpdateCategoryRequest UpdateParentCategoryRequest => new()
    {
        Name = "Test category",
        ExternalId = Guid.Parse("e6aea115-f989-4d55-9df8-d2599a6bbf00")
    };

    public static UpdateCategoryRequest UpdateChildCategoryRequest => new()
    {
        Name = "Child category",
        ExternalId = Guid.Parse("1d8e6450-ab19-451b-bc82-9be17228f601"),
        ParentId = UpdateParentCategoryRequest.ExternalId
    };
}