using Npgsql;

namespace Turg.App.Infrastructure;

internal class SqlConnectionFactory
{
    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(Constants.ConnectionString);
    }
}
