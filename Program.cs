using Microsoft.Extensions.DependencyInjection;
using MyGreeter;

Console.WriteLine("Hi");

var services = new ServiceCollection();
services.AddSingleton<IGreeter, Greeter>();

var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Press any key to continue or 'q' to exit!");

char key = Console.ReadKey().KeyChar;

while (key != 'q')
{
    using var scope = serviceProvider.CreateScope();

    var greeter = scope.ServiceProvider.GetService<IGreeter>();
    var greeter2 = scope.ServiceProvider.GetService<IGreeter>();

    var message = greeter.Greet();

    Console.WriteLine(message);

    key = Console.ReadKey().KeyChar;
}

serviceProvider.Dispose();
