using System.Diagnostics;
using Turg.App.Services;
using Turg.App.Services.HostedServices;

namespace Turg.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("::Program:: Main - Current process id is {0}", Process.GetCurrentProcess().Id);

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder => { webBuilder.UseStartup<Startup>(); })
                    .ConfigureServices((services) =>
                    {
                        services.AddSingleton<SMSService>();
                        services.AddSingleton<EmailService>();
                        services.AddHostedService<OrderNotificationBackgroundService>();
                        services.AddHostedService<StockNotificationBackgroundService>();
                    })
             .Build()
             .Run();
        }
    }
}
