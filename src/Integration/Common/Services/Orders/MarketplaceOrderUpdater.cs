using AutoMapper;
using Infrastructure;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Common.Services.Orders;

public abstract class MarketplaceOrderUpdater
{
    protected readonly Marketplace Marketplace;
    protected readonly IServiceProvider ServiceProvider;
    protected readonly IMapper Mapper;

    protected MarketplaceOrderUpdater(Marketplace marketplace, IServiceProvider serviceProvider, IMapper mapper)
    {
        Marketplace = marketplace;
        ServiceProvider = serviceProvider;
        Mapper = mapper;
    }

    public abstract Task UpdateAsync();

    protected Task<List<string>> GetOrderNumbersToUpdate(int marketplaceId)
    {
        var context = ServiceProvider.GetRequiredService<MContext>();

        return context.Orders
            .AsNoTracking()
            .Where(o => o.MarketplaceId == marketplaceId && o.Status != OrderStatus.Canceled &&
                        o.Status != OrderStatus.Finished)
            .Select(o => o.Number)
            .ToListAsync();
    }
}