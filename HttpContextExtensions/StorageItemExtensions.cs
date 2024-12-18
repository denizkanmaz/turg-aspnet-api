using Turg.App.StorageItems;

namespace Turg.App.HttpContextExtensions.StorageItemExtensions;

internal static class HttpContextStorageItemExtensions
{
    internal static void SetItem<T>(this HttpContext context, T item) where T : StorageItem
    {
        // typeof(T) = namespace + class
        // typeof(ClientMetadata) = Turg.App.Middlewares.ClientMetadataParsing.ClientMetadata
        context.Items[typeof(T)] = item;
    }
    
    internal static T GetItem<T>(this HttpContext context) where T : StorageItem
    {
        return context.Items[typeof(T)] as T;
    }
}
