using UAParser;

namespace Turg.App.Middlewares.ClientMetadataParsing;

internal class ClientMetadata
{
    public ClientMetadata(ClientInfo clientInfo)
    {
        Device = new Device
        {
            Brand = clientInfo.Device.Brand,
            Model = clientInfo.Device.Model,
            IsMobile = clientInfo.OS.Family.Contains("Android") || clientInfo.OS.Family.Contains("iOS")
        };
        Browser = new Browser
        {
            Family = clientInfo.UA.Family,
            Version = $"{clientInfo.UA.Major}.{clientInfo.UA.Minor}.{clientInfo.UA.Patch}"
        };
        OperaringSystem = new OperatingSystem
        {
            Family = clientInfo.OS.Family,
            Version = $"{clientInfo.OS.Major}.{clientInfo.OS.Minor}.{clientInfo.OS.Patch}"
        };
    }

    public Device Device { get; }
    public OperatingSystem OperaringSystem { get; }
    public Browser Browser { get; }
}

internal class Device
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public bool IsMobile { get; set; }
}

internal class OperatingSystem
{
    public string Family { get; set; }
    public string Version { get; set; }
}

internal class Browser
{
    public string Family { get; set; }
    public string Version { get; set; }
}
