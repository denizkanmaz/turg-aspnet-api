using System.Diagnostics;

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
             .Build()
             .Run();
        }
    }
}
