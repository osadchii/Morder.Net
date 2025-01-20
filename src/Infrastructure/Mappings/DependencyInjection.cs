using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mappings;

public static class AutoMapperDependencyInjection
{
    public static void AddMorderAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new CompanyProfile());
            mc.AddProfile(new BotProfile());
            mc.AddProfile(new MarketplaceProfile());
            mc.AddProfile(new OrderProfile());
        });

        var mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}