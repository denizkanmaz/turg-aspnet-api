using System.Data.Common;

namespace Turg.App.Infrastructure;

public interface ISqlCommandExecutor
{
    Task<T> ExecuteReaderAsync<T>(string commandText, Func<DbDataReader, Task<T>> readerFunc, params DbParameter[] parameters);
    Task ExecuteNonQueryAsync(string commandText, params DbParameter[] parameters);
}
