using AutoMapper;
using Infrastructure.MediatR.Categories.Commands;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.Models.Companies;
using Infrastructure.Models.Products;

namespace Infrastructure.Mappings;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<UpdateCompanyInformationRequest, Company>();
        CreateMap<Company, CompanyDto>();

        CreateMap<Category, CategoryDto>()
            .ForMember(m => m.ParentId,
                opt => opt.MapFrom(c => c.Parent == null ? Guid.Empty : c.Parent.ExternalId));
        CreateMap<UpdateCategoryRequest, Category>()
            .ForMember(e => e.ParentId,
                opt => opt.Ignore());
    }
}