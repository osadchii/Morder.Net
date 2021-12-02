using Infrastructure.Models.Interfaces;

namespace Infrastructure.Cache.Interfaces;

public interface IIdExtractor<T> where T : IHasId, IHasExternalId
{
    Task<int?> GetIdAsync(Guid externalId);
}