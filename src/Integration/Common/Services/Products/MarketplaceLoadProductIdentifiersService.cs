using AutoMapper;
using Infrastructure.Models.Marketplaces;

namespace Integration.Common.Services.Products;

public abstract class MarketplaceLoadProductIdentifiersService
{
    protected readonly IMapper Mapper;
    protected readonly IServiceProvider ServiceProvider;

    protected MarketplaceLoadProductIdentifiersService(IMapper mapper, IServiceProvider serviceProvider)
    {
        Mapper = mapper;
        ServiceProvider = serviceProvider;
    }

    public abstract Task LoadProductIdentifiersAsync(Marketplace marketplace);
}