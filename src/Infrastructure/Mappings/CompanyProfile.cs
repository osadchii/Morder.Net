using AutoMapper;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.MediatR.PriceTypes.Commands;
using Infrastructure.MediatR.Products.Commands;
using Infrastructure.MediatR.Warehouses.Commands;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Prices;
using Infrastructure.Models.Products;
using Infrastructure.Models.Warehouses;

namespace Infrastructure.Mappings;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<UpdateCompanyInformationRequest, Company>();
        CreateMap<Company, CompanyDto>();

        CreateMap<UpdateWarehouseRequest, Warehouse>();
        CreateMap<Warehouse, WarehouseDto>();

        CreateMap<UpdatePriceTypeRequest, PriceType>();
        CreateMap<PriceType, PriceTypeDto>();

        CreateMap<Category, CategoryDto>()
            .ForMember(m => m.ParentId,
                opt => opt.MapFrom(c => c.Parent == null ? Guid.Empty : c.Parent.ExternalId));
        CreateMap<UpdateCategoryRequest, Category>()
            .ForMember(e => e.ParentId,
                opt => opt.Ignore());

        CreateMap<Product, ProductDto>()
            .ForMember(e => e.CategoryId,
                opt => opt.MapFrom(p => p.Category == null ? Guid.Empty : p.Category.ExternalId));
        CreateMap<UpdateProductRequest, Product>()
            .ForMember(e => e.CategoryId,
                opt => opt.Ignore());
    }
}