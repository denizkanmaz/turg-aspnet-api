namespace Turg.App.Services;

internal class EmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    internal void Send()
    {
        _logger.LogInformation("::EmailService:: Send - Email sent!");
    }
}
