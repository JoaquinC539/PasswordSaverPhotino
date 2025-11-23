namespace PasswordSaver.Utils;

public static class Utils
{
    public static string GetPlatform()
    {
        return DeviceInfo.Current.Platform.ToString();
    }
    public static async Task<bool> CopierDbAsync(FileResult fileGet, string destPath)
    {
        try
        {
            if(fileGet == null)
            {
                return false;
            }
            using Stream pickedStream = await fileGet.OpenReadAsync();
            using FileStream destStream = File.Create(destPath);
            await pickedStream.CopyToAsync(destStream);
            return true;
        }
        catch (System.Exception e)
        {
            
            throw new Exception($"Exception ocurred at copying Db fileresult stream {e.Message}");
        }
    }
    public static async Task<bool> CopierDbAsync(string originPathFile, string destPathFile)
    {
        try
        {
            if (!File.Exists(originPathFile))
            {
                return false;
            }
            File.Copy(originPathFile,destPathFile);
            return true;
        }
        catch (System.Exception e)
        {
            
            throw new Exception($"Exception ocurred at copying Db origin path stream {e.Message}");
        }
    }
    public static async Task<bool> CopierDbStreamAsync(string originPathFile, string destPathFile)
    {
        try
        {
            if (!File.Exists(originPathFile))
            {
                return false;
            }
            using FileStream sourceStream = new FileStream(originPathFile,FileMode.Open,FileAccess.Read);
            using FileStream destStream = File.Create(destPathFile);
            sourceStream.CopyTo(destStream);
            return true;
        }
        catch (System.Exception e)
        {
            
            throw new Exception($"Exception ocurred at copying Db origin path stream {e.Message}");
        }
    }
}