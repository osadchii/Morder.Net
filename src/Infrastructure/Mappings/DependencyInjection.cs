using AutoMapper;
using Infrastructure.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.MediatR;

public static class AutoMapperDependencyInjection
{
    public static void AddMorderAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new CompanyProfile()); });

        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}