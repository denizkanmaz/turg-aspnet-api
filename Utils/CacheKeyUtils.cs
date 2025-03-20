using System.Security.Cryptography;
using System.Text;

namespace Turg.App.Utils;

internal static class CacheKeyUtils
{
    public static string GenerateCacheKey(HttpRequest request)
    {
        var queries = request.Query.OrderBy(x => x.Key);

        var keyBuilder = new StringBuilder();
        keyBuilder.Append(request.Path);

        foreach (var query in queries)
        {
            keyBuilder.Append($"|{query.Key}-{query.Value}");
        }

        using (var md5 = MD5.Create())
        {
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(keyBuilder.ToString()));
            var hashedKey = Convert.ToBase64String(hashBytes);

            return hashedKey;
        }
    }
}
