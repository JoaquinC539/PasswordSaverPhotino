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

    private string UiFolderName="browser";

    private string rootpath;

    private string HostUrl => $"http://localhost:{port}";
    public string BaseUrl => $"{HostUrl}/index.html";
    
    private static LocalWebServer? instance = null;

    private LocalWebServer()
    {
        // port = GetFreePort();
        rootpath = GetStaticUiRootPath(UiFolderName);
        webServer = CreateWebServer(HostUrl,rootpath);
        Console.WriteLine($"Using this host: {HostUrl}");
        
        
        
    }

    public static LocalWebServer GetLocalWebServer()
    {
        if(instance == null)
        {
            instance = new LocalWebServer();
        }
        return instance;
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
            return 7614;
        }
    }

    public string GetStaticUiRootPath(string uiFolder)
    {
        return Path.Combine(AppContext.BaseDirectory,uiFolder);
    }

    private WebServer CreateWebServer(string url, string rootpath)
    {
        Console.WriteLine($"Server from: {url} - {rootpath}");
        WebServer server = new WebServer( o => o
        .WithUrlPrefix(url)
        .WithMode(HttpListenerMode.EmbedIO));
        server.WithModule(GetCorsModule());
        server.WithWebApi("/api",m=>m
        .WithController<ApiController>());
        server.WithStaticFolder("/",rootpath,true);

        return server;
    }

    private CorsModule GetCorsModule()
    {
        return new CorsModule("/",
        origins: "*", headers: "*",methods:"*"
        );
    }

    public void StartServer()
    {
        try
        {
            webServer.RunAsync();
            Console.WriteLine("Server started");

            return ;
        }
        catch (System.Exception e) 
        {
            Console.WriteLine($"An exception occured: {e.Message}");
            
        }
    }
}