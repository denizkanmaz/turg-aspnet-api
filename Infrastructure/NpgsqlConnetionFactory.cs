using System.Data.Common;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Turg.App.Infrastructure;

public class NpgsqlConnectionFactory : ISqlConnectionFactory
{
    private readonly SqlConnectionOptions _options;

    public NpgsqlConnectionFactory(IOptions<SqlConnectionOptions> options)
    {
        _options = options.Value;
        Console.WriteLine("NpgsqlConnectionFactory:ctor");
    }

    public DbConnection CreateConnection()
    {
        return new NpgsqlConnection(_options.ConnectionString);
    }
}
