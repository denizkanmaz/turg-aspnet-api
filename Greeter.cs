namespace MyGreeter;

public class Greeter : IGreeter, IDisposable
{
    public Greeter()
    {
        Console.WriteLine("Instance is created!");
    }

    public void Dispose()
    {
        Console.WriteLine("Instance is getting disposed!");
    }

    public string Greet()
    {
        return "Hello world!";
    }
}
