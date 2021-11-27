using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Extensions;

public static class CommonExtensions
{
    public static string ToJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static Task<IHasExternalId?> GetByExternalIdAsync(this IQueryable<IHasExternalId> queryable, Guid id)
    {
        return queryable.SingleOrDefaultAsync(e => e.ExternalId == id);
    }
}