using Npgsql;

namespace Turg.App.Infrastructure;

public class SqlCommandExecutor
{
    private readonly SqlConnectionFactory _connectionFactory;

    public SqlCommandExecutor(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // Higher Order Function (HOF)
    internal async Task<T> ExecuteReaderAsync<T>(string commandText, Func<NpgsqlDataReader, Task<T>> readerFunc, params NpgsqlParameter[] parameters)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await using var cmd = new NpgsqlCommand
        {
            Connection = conn,
            CommandText = commandText,
        };

        if (parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        await conn.OpenAsync();

        await using var reader = await cmd.ExecuteReaderAsync();

        var result = await readerFunc(reader);

        await reader.CloseAsync();
        await conn.CloseAsync();

        return result;
    }

    internal async Task ExecuteNonQueryAsync(string commandText, params NpgsqlParameter[] parameters)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await using var cmd = new NpgsqlCommand
        {
            Connection = conn,
            CommandText = commandText,
        };

        if (parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }

        await conn.OpenAsync();

        await cmd.ExecuteNonQueryAsync();

        await conn.CloseAsync();
    }
}
