using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

public interface IMigrationService
{
    void Migrate();
}

public class MigrationService : IMigrationService
{
    private readonly MContext _mContext;
    private readonly ILogger<MigrationService> _logger;

    public MigrationService(MContext mContext, ILogger<MigrationService> logger)
    {
        _mContext = mContext;
        _logger = logger;
    }

    public void Migrate()
    {
        Task task = _mContext.Database.MigrateAsync();
        Task.WaitAny(task);

        _logger.LogInformation("Migration completed");
    }
}