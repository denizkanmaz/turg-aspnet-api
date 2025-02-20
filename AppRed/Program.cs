using System.Reflection;

var appBlueAssembly = Assembly.LoadFrom(Path.Combine("..", "AppBlue", "bin", "Debug", "net8.0", "AppBlue.dll"));

var greetingsServiceType = appBlueAssembly.GetType("AppBlue.Services.GreetingsService");

var greetingsService = Activator.CreateInstance(greetingsServiceType);

var greetMethod = greetingsServiceType.GetMethod("Greet");

var resultFromAppBlue = greetMethod.Invoke(greetingsService, null);

Console.WriteLine(resultFromAppBlue);
