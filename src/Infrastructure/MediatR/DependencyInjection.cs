using Infrastructure.MediatR.Companies.Commands;
using Infrastructure.MediatR.Companies.Handlers;
using Infrastructure.MediatR.Companies.Queries;
using Infrastructure.Models.Companies;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MediatR;

public static class MediatRDependencyInjection
{
    public static void AddMorderMediatR(this IServiceCollection services)
    {
        services.AddMediatR(typeof(UpdateCompanyInformationHandler).Assembly);
        services.AddTransient<IRequestHandler<UpdateCompanyInformation, Unit>,
            UpdateCompanyInformationHandler>();
        services.AddTransient<IRequestHandler<GetCompanyInformation, CompanyDto>,
            GetCompanyInformationHandler>();
    }
}