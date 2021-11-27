using Infrastructure.MediatR.Companies.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MediatR;

public static class MediatRDependencyInjection
{
    public static void AddMorderMediatR(this IServiceCollection services)
    {
        services.AddMediatR(typeof(UpdateCompanyInformationHandler).Assembly);
    }
}