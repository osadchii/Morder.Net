using AutoMapper;
using Infrastructure.Models.Marketplaces;

namespace Integration.Common.Services.Products;

public abstract class MarketplaceLoadProductIdsService
{
    protected readonly IMapper Mapper;
    protected readonly IServiceProvider ServiceProvider;

    protected MarketplaceLoadProductIdsService(IMapper mapper, IServiceProvider serviceProvider)
    {
        Mapper = mapper;
        ServiceProvider = serviceProvider;
    }

    public abstract Task<Dictionary<string, string>> LoadProductIds(Marketplace marketplace);
}