namespace PasswordSaver.Utils;

public static class Utils
{
    public static string GetPlatform()
    {
        return DeviceInfo.Current.Platform.ToString();
    }
}