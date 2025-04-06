using System.Data.Common;
using Npgsql;

namespace Turg.App.Infrastructure;

public class NpgsqlCommandExecutor : ISqlCommandExecutor
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public NpgsqlCommandExecutor(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    // Higher Order Function (HOF)
    public async Task<T> ExecuteReaderAsync<T>(string commandText, Func<DbDataReader, Task<T>> readerFunc, params DbParameter[] parameters)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await using var cmd = new NpgsqlCommand
        {
            Connection = conn as NpgsqlConnection,
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

    public async Task ExecuteNonQueryAsync(string commandText, params DbParameter[] parameters)
    {
        await using var conn = _connectionFactory.CreateConnection();
        await using var cmd = new NpgsqlCommand
        {
            Connection = conn as NpgsqlConnection,
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
