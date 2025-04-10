using System.Data.Common;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Turg.App.Infrastructure;

public class NpgsqlConnectionFactory(IOptions<SqlConnectionOptions> options) : ISqlConnectionFactory
{
    public DbConnection CreateConnection()
    {
        return new NpgsqlConnection(options.Value.ConnectionString);
    }
}
