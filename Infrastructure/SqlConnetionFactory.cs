using Microsoft.Extensions.Options;
using Npgsql;

namespace Turg.App.Infrastructure;

public class SqlConnectionOptions
{
    public string ConnectionString { get; set; }
}

public class SqlConnectionFactory
{
    private readonly SqlConnectionOptions _options;

    public SqlConnectionFactory(IOptions<SqlConnectionOptions> options)
    {
        _options = options.Value;
        Console.WriteLine("SqlConnectionFactory:ctor");
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_options.ConnectionString);
    }
}
