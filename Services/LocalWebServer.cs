using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Cors;
using EmbedIO.WebApi;
using PasswordSaver.Controllers;

namespace PasswordSaver.Services;

public class LocalWebServer
{

    private WebServer webServer;
    private int port = 7614;

    private string UiFolderName;

    private string rootpath;

    private string HostUrl => $"http://localhost:{port}";
    public string BaseUrl => $"{HostUrl}/index.html";
    
    private static LocalWebServer? instance = null;

    private LocalWebServer()
    {
        UiFolderName = "browser";
        // DEV Coment port to set fixed port 7614
        port = GetFreePort();   
        rootpath = GetStaticUiRootPath(UiFolderName);
        webServer = CreateWebServer(HostUrl, rootpath);
        Console.WriteLine($"Using this host: {HostUrl}");

    }

    public static LocalWebServer GetLocalWebServer()
    {
        if (instance == null)
        {
            instance = new LocalWebServer();
        }
        return instance;
    }

    public string GetStaticUiRootPath(string uiFolder)
    {
#if ANDROID || IOS
        string rootpath = Path.Combine(FileSystem.AppDataDirectory, uiFolder);
#else
        string rootpath = Path.Combine(AppContext.BaseDirectory, uiFolder);
#endif
        Console.WriteLine($"Serving UI from {rootpath}");
        return rootpath;
    }
    
    private  WebServer CreateWebServer(string url, string rootpath)
    {
        Console.WriteLine("Server from: " + url + " - " + rootpath);
        WebServer server = new WebServer(o => o
        .WithUrlPrefix(url)
        .WithMode(HttpListenerMode.EmbedIO));
        server.WithModule(GetCorsModule());        
        server
        .WithWebApi("/api",m=> m
            .WithController<ApiController>()
            )
        .WithStaticFolder("/", rootpath, true);
        
        return server;
    }
    private static int GetFreePort()
    {
        try
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
        catch (System.Exception e)
        {
            Console.WriteLine("Error at getting free port: " + e.Message);
            return 5467;
        }

    }

    public void StartServer()
    {
        try
        {
            webServer.RunAsync();
            Console.WriteLine("Server started");

            return;
        }
        catch (System.Exception e)
        {

            Console.WriteLine($"An exception occured: {e.Message}");
        }
    }
    
   private CorsModule GetCorsModule()
    {
        return new CorsModule("/",
        origins: "*", headers: "*",methods:"*"
        );
    }
    
}
