using Microsoft.Extensions.Logging;
using PasswordSaver.Database;
using PasswordSaver.Interfaces;
using PasswordSaver.Services;

namespace PasswordSaver;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        builder.Services.AddSingleton<MasterPasswordService>();

#if WINDOWS
         builder.Services.AddTransient<IFolderPicker,Platforms.Windows.FolderPickerService>();
#elif ANDROID
        builder.Services.AddTransient<IFolderPicker,Platforms.Android.FolderPickerService>();
        builder.Services.AddTransient<ISafService,Platforms.Android.SafService>();
#endif

        DB db = DB.GetDB();
        db.CreateOrCheckTables();
        return builder.Build();
    }
}