namespace Api.BackgroundServices;

public abstract class BackgroundService : IHostedService, IDisposable
{
    protected readonly IServiceProvider Services;
    protected int TimerInterval { get; set; }
    private readonly ILogger _logger;
    private readonly string _serviceName;
    private Task? _task;

    private Timer _timer = null!;

    protected BackgroundService(ILogger logger, IServiceProvider services, string serviceName)
    {
        _logger = logger;
        Services = services;
        _serviceName = serviceName;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{_serviceName} service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(TimerInterval));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        if (_task is null
            || _task.Status is TaskStatus.Canceled or TaskStatus.Faulted or TaskStatus.RanToCompletion)
        {
            _task = ServiceWork();
        }
    }

    protected abstract Task ServiceWork();

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{_serviceName} service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}