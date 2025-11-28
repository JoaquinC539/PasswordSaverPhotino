public interface ISafService
{
    Task<bool> BackUpToExternalFolderAsync(string uriString);
}