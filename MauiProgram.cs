using Microsoft.Extensions.Logging;
using PasswordSaver.Database;
using PasswordSaver.Interfaces;

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

#if WINDOWS
         builder.Services.AddTransient<IFolderPicker,Platforms.Windows.FolderPickerService>();
#endif

        DB db = DB.GetDB();
        db.CreateOrCheckTables();
        return builder.Build();
    }
}