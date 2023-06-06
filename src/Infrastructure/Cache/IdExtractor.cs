using Infrastructure.Cache.Interfaces;
using Infrastructure.Models;
using Infrastructure.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Cache;

public class IdExtractor<T> : IIdExtractor<T> where T : BaseEntity, IHasId, IHasExternalId
{
    private readonly MContext _context;
    private readonly IEntityIdCacheService<T> _cacheService;

    public IdExtractor(MContext context, IEntityIdCacheService<T> cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<int?> GetIdAsync(Guid externalId)
    {
        if (_cacheService.TryGetValue(externalId, out var id))
        {
            return id;
        }

        var dbEntry = await _context.Set<T>().AsNoTracking()
            .Select(e => new { e.Id, e.ExternalId })
            .SingleOrDefaultAsync(e => e.ExternalId == externalId);

        if (dbEntry is null)
        {
            return null;
        }

        _cacheService.Set(externalId, dbEntry.Id);
        return dbEntry.Id;
    }
}