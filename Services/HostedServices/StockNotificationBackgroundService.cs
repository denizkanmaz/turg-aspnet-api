namespace Turg.App.Services.HostedServices;

internal class StockNotificationBackgroundService : BackgroundService
{
    private readonly ILogger<StockNotificationBackgroundService> _logger;
    private readonly SMSService _smsService;

    public StockNotificationBackgroundService(ILogger<StockNotificationBackgroundService> logger, SMSService smsService)
    {
        _logger = logger;
        _smsService = smsService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Database checked!");
            _smsService.Send();

            await Task.Delay(5000);
        }
    }
}
