using System.Web;
using PasswordSaver.Services;
namespace PasswordSaver;

public partial class WebViewPage :ContentPage
{
    private LocalWebServer localWebServer;


    public WebViewPage()
    {
        InitializeComponent();
        WebViewElement.Navigating += NavigatingHandler;
        localWebServer = LocalWebServer.GetLocalWebServer();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        localWebServer = LocalWebServer.GetLocalWebServer();
        Initialize();
    }

    private void Initialize()
    {
        try
        {
            localWebServer.StartServer();
            Console.WriteLine("Starting app in: " + localWebServer.BaseUrl);
            WebViewElement.Source = new UrlWebViewSource { Url = localWebServer.BaseUrl };
            // DEV set your IP url or localhost
            // WebViewElement.Source = new UrlWebViewSource { Url = $"http://localhost:4200/?cachebuster={DateTime.Now.Ticks}" };        

        }
        catch (System.Exception e)
        {
            Console.WriteLine("Exception at starting server: " + e.Message);
            throw new Exception("Error at generating webview :"+e.Message);
        }

    }

    public void NavigatingHandler(object? sender, WebNavigatingEventArgs e)
    {
        if (!e.Url.Contains("cachebuster"))
        {
            e.Cancel = true;
            UriBuilder urlBuilder = new UriBuilder(e.Url);
            var query = HttpUtility.ParseQueryString(urlBuilder.Query);
            query["cachebuster"] = DateTime.Now.Ticks.ToString();
            urlBuilder.Query = query.ToString();
            string finalUrl = urlBuilder.ToString();
            WebViewElement.Source = new UrlWebViewSource { Url = finalUrl };
        }
    }

}


