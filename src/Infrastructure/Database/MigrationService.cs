using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    private readonly IConfiguration _configuration;

    public MigrationService(MContext mContext, ILogger<MigrationService> logger, IConfiguration configuration)
    {
        _mContext = mContext;
        _logger = logger;
        _configuration = configuration;
    }

    public void Migrate()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        _logger.LogInformation($"Connection string: {connectionString}");

        Task task = _mContext.Database.MigrateAsync();
        Task.WaitAny(task);

        _logger.LogInformation("Migration completed");
    }
}