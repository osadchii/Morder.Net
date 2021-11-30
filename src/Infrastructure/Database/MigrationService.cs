using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public interface IMigrationService
{
    void Migrate();
}

public class MigrationService : IMigrationService
{
    private readonly MContext _mContext;

    public MigrationService(MContext mContext)
    {
        _mContext = mContext;
    }

    public void Migrate()
    {
        Task task = _mContext.Database.MigrateAsync();
        Task.WaitAny(task);
    }
}