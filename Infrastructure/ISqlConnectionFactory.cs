namespace Turg.App.Infrastructure;

using System.Data.Common;

public interface ISqlConnectionFactory
{
    DbConnection CreateConnection();
}
