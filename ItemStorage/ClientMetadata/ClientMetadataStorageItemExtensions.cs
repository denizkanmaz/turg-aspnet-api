namespace Turg.App.ItemStorage.ClientMetadata;

internal static class ClientMetadataStorageItemExtensions
{
    private const string CLIENT_METADATA_STORAGE_ITEM_KEY = "ClientMetadata";
    internal static void SetClientMetadata(this HttpContext context, ClientMetadataStorageItem clientMetadata)
    {
        context.Items[CLIENT_METADATA_STORAGE_ITEM_KEY] = clientMetadata;
    }

    internal static ClientMetadataStorageItem GetClientMetadata(this HttpContext context)
    {
        return context.Items[CLIENT_METADATA_STORAGE_ITEM_KEY] as ClientMetadataStorageItem;
    }
}
