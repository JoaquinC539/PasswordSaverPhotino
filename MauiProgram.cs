using Microsoft.Extensions.Logging;
using PasswordSaver.Database;

namespace PasswordSaver;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		Console.WriteLine("Pre ejecucion de maui builder");
		DB db = DB.GetDB();
		db.CreateOrCheckTables();
		return builder.Build();
	}
}
