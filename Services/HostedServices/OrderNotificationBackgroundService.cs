
namespace Turg.App.Services.HostedServices;

internal class OrderNotificationBackgroundService : BackgroundService
{
    private readonly EmailService _emailService;
    private readonly ILogger<OrderNotificationBackgroundService> _logger;
    public OrderNotificationBackgroundService(EmailService emailService, ILogger<OrderNotificationBackgroundService> logger, IHostApplicationLifetime hostApplicationLifetime)
    {
        _emailService = emailService;
        _logger = logger;

        hostApplicationLifetime.ApplicationStarted.Register(() =>
        {
            _logger.LogInformation("::OrderNotificationBackgroundService:: - Service started!");
        });

        hostApplicationLifetime.ApplicationStopping.Register(() =>
        {
            _logger.LogInformation("::OrderNotificationBackgroundService:: - Service is getting stopped!");
        });

        hostApplicationLifetime.ApplicationStopped.Register(() =>
        {
            _logger.LogInformation("::OrderNotificationBackgroundService:: - Service stopped!");
        });
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("::OrderNotificationBackgroundService:: ExecuteAsync - Data fetched from db - {0}", DateTime.UtcNow.ToLongTimeString());
            _emailService.Send();

            await Task.Delay(2000);
        }
    }
}
