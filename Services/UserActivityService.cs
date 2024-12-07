namespace Turg.App.Services;

internal class UserActivityService
{
    private readonly ILogger<UserActivityService> _logger;
    public UserActivityService(ILogger<UserActivityService> logger)
    {
        _logger = logger;
        _logger.LogInformation("UserActivityService created at {Timestamp}", DateTime.UtcNow);
    }

    internal void StartActivity()
    {
        _logger.LogInformation("Activity started");
    }
}
