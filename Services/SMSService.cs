namespace Turg.App.Services;

internal class SMSService
{
    private readonly ILogger<SMSService> _logger;

    public SMSService(ILogger<SMSService> logger)
    {
        _logger = logger;
    }

    internal void Send()
    {
        _logger.LogInformation("SMS sent!");
    }
}
