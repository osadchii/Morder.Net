using Infrastructure.Cache.Interfaces;
using Infrastructure.Models.BotUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

public class BotUsersCache : IBotUsersCache
{
    private readonly IMemoryCache _cache;
    private readonly MContext _context;
    private const string BaseKey = "UserBotCache_";

    public BotUsersCache(IMemoryCache cache, MContext context)
    {
        _cache = cache;
        _context = context;
    }

    public async Task<BotUser?> GetUserAsync(long chatId)
    {
        if (!_cache.TryGetValue(CacheKey(chatId), out BotUser? user))
        {
            user = await _context.BotUsers
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.ChatId == chatId);

            if (user is not null)
            {
                Set(chatId, user);
            }
        }

        return user;
    }

    public void Set(long chatId, BotUser user)
    {
        _cache.Set(CacheKey(chatId), user);
    }

    public void Remove(long chatId)
    {
        _cache.Remove(CacheKey(chatId));
    }

    private static string CacheKey(long chatId) => $"{BaseKey}{chatId}";
}