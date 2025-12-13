namespace PasswordSaver.Services;


public static class ServiceHelper
{
    public static T? GetService<T>()
    {

        return Current.GetService<T>();
    } 

    public static IServiceProvider Current =>
        Application.Current!.Windows[0]!.Handler!.MauiContext!.Services;
}

