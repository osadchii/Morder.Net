using Infrastructure.Models.Marketplaces.Kuper;
using Integration.Kuper.Feeds;

namespace Integration.Kuper.Clients.Offers;

public interface IKuperOfferClient
{
    Task SendCategories(KuperDto kuper, KuperCategoryFeed categories);
    Task SendOffers(KuperDto kuper, KuperProductFeed offers);
}

public class KuperOfferClient : KuperClientBase, IKuperOfferClient
{
    public Task SendCategories(KuperDto kuper, KuperCategoryFeed categories)
    {
        return PostAsync(kuper, "/v1/import/categories", categories);
    }
    
    public Task SendOffers(KuperDto kuper, KuperProductFeed offers)
    {
        return PostAsync(kuper, "/v1/import/offers", offers);
    }
}