using AutoMapper;
using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.Models.Companies;

namespace Infrastructure.Mappings;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<UpdateCompanyInformation, Company>();
    }
}