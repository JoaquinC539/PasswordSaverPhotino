


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PasswordSaver.Utils;
using PasswordSaver.Database;
using PasswordSaver.Services;
namespace PasswordSaver;

public class Program
{

    private static IHostBuilder  builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
    {
     services.AddSingleton<MasterPasswordService>();
     services.AddSingleton<PasswordService>();   
    });

    
    
    [STAThread]
    public static void Main(string[] args)
    {
        bool IsLinux = OperatingSystem.IsLinux();
        if (!IsLinux)
        {
            Console.WriteLine("Not supported platform, only linux");
            return;
        }
        try
        {
            DB db = DB.GetDB();
            db.CreateOrCheckTables();
            LocalWebServer webServer = LocalWebServer.GetLocalWebServer();
            webServer.StartServer();
            WebKit.Module.Initialize();
            using IHost host = builder.Build();
            var application = Gtk.Application.New("com.JCOpenSoftware.PasswordSaver",Gio.ApplicationFlags.FlagsNone);
            ServiceLocator.ServiceProvider = host.Services;            
            application.OnActivate += (sender, _) =>
            {
                
                var webView=WebKit.WebView.New();   
                webView.WidthRequest=500;            
                webView.HeightRequest=300;     
                webView.LoadUri(webServer.BaseUrl);
                Gtk.Window.SetDefaultIconName("com.JCOpenSoftware.PasswordSaver");
                
                var window = Gtk.ApplicationWindow.New((Gtk.Application) sender);
                window.Title = "Password Saver";
                var iconsPath = Path.Combine(AppContext.BaseDirectory,"Resources","AppIcon"); 
                var iconTheme = Gtk.IconTheme.GetForDisplay(window.Display);
                iconTheme.AddSearchPath(iconsPath);
                window.SetIconName("com.JCOpenSoftware.PasswordSaver");                
                window.SetChild(webView);
                window.Resizable=true;
                window.DefaultWidth=1200;
                window.DefaultHeight=900;                
                window.Show();
            };
            application.RunWithSynchronizationContext(null);


        }
        catch (System.Exception)
        {

            Console.WriteLine("An exception occurred it might have to be because it requires webkit 6");
            Console.WriteLine("Please run sudo apt install libwebkitgtk-6.0 and try again");
            throw;
        }
    }
}



