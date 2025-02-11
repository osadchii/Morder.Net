using Integration.Common.Services.Orders;

namespace Api.BackgroundServices.Marketplaces;

public class SendOrderConfirmationNotifications: BackgroundService
{
    public SendOrderConfirmationNotifications(ILogger<SendOrderConfirmationNotifications> logger, IServiceProvider services) : base(logger, services)
    {
        TimerInterval = 5;
    }

    protected override async Task ServiceWork()
    {
        await using var scope = Services.CreateAsyncScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<ISendNotificationService>();

        await notificationService.SendNotifications();
    }
}