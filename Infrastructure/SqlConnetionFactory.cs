using Npgsql;

namespace Turg.App.Infrastructure;

public class SqlConnectionFactory
{
    public SqlConnectionFactory()
    {
        Console.WriteLine("SqlConnectionFactory:ctor");
    }
    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(Constants.ConnectionString);
    }
}
