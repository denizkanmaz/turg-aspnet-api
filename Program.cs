using System.Diagnostics;

namespace Turg.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("MyProgram.cs - Current process id is {0}", Process.GetCurrentProcess().Id);
            CreateWebHostBuilder(args)
            .Build()
            .Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(
                    webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
