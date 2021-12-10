using Infrastructure.Models.BotUsers;

namespace Infrastructure.Cache.Interfaces;

public interface IBotUsersCache
{
    Task<BotUser?> GetUserAsync(long chatId);
    void Set(long chatId, BotUser user);
    void Remove(long chatId);
}