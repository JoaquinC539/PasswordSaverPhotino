using Android.Content.Res;

namespace PasswordSaver.Platform.Android;

public class CopyUiToAppData
{
    private static string AssetFolder = "browser";


    public static void Copy(AssetManager? assets)
    {
        if (assets == null)
        {
            Console.WriteLine("Assets null");
            return;
        }
        string appDataDir = Path.Combine(FileSystem.AppDataDirectory, AssetFolder);
        CopyAssetsFolder(assets, AssetFolder, appDataDir);
        Console.WriteLine($"Copied assets from {AssetFolder} to {appDataDir}");
    }


    private static void CopyAssetsFolder(AssetManager assets, string assetFolderPath, string destFolderPath)
    {
        Console.WriteLine("Executing assets copier...");
        Directory.CreateDirectory(destFolderPath);

        string[] assetsList = assets.List(assetFolderPath) ?? [];

        foreach (var asset in assetsList)
        {
            var assetPath = Path.Combine(assetFolderPath, asset);
            var destPath = Path.Combine(destFolderPath, asset);
            var subFiles = assets.List(assetPath);
            if (subFiles != null && subFiles.Length > 0)
            {
                CopyAssetsFolder(assets, assetPath, destPath);
            }
            else
            {

                using var assetStream = assets.Open(assetPath);
                using var destStream = File.Create(destPath);
                assetStream.CopyTo(destStream);
            }
        }
    }
}
