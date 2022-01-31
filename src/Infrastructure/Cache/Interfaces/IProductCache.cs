namespace Infrastructure.Cache.Interfaces;

public interface IProductCache
{
    Task<Dictionary<string, int>> GetProductIdsByArticul(List<string> articuls, bool ignoreNotFound = false);
}