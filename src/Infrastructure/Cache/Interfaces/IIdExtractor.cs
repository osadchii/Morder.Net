using Infrastructure.Models.Interfaces;

namespace Infrastructure.Cache.Interfaces;

// ReSharper disable once UnusedTypeParameter
public interface IIdExtractor<T> where T : IHasId, IHasExternalId
{
    Task<int?> GetIdAsync(Guid externalId);
}