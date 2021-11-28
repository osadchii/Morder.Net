using Infrastructure.Models.Companies;
using MediatR;

namespace Infrastructure.MediatR.Companies.Queries;

public class GetCompanyInformationRequest : IRequest<CompanyDto>
{
}