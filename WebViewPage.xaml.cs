using System.Web;
using PasswordSaver.Services;
namespace PasswordSaver;

public partial class WebViewPage : ContentPage
{
    private LocalWebServer localWebServer;
    private WebView WebViewElement;

    public WebViewPage()
    {

        InitializeComponent();
        localWebServer = LocalWebServer.GetLocalWebServer();
        BuildPageLayout();
        
        // WebViewElement.Navigating += NavigatingHandler;
        // localWebServer = LocalWebServer.GetLocalWebServer();
    }



    protected override void OnAppearing()
    {
        base.OnAppearing();

        Initialize();
        // localWebServer = LocalWebServer.GetLocalWebServer();
        // BuildPageLayout();
        // Initialize();

    }

    private void BuildPageLayout()
    {
        WebViewElement = new WebView
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        Content = CreatePlatformLayout();

    }

    private View CreatePlatformLayout()
    {
#if ANDROID
            return CreateAndroidLayout();
#else
        return CreateOtherPlatformsLayout();
#endif
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
            throw new Exception("Error at generating webview :" + e.Message);
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

    private Grid CreateAndroidLayout()
    {
        var mainGrid = new Grid();
        var boxView = new BoxView();
        boxView.BackgroundColor = Colors.Transparent;
        boxView.HeightRequest = 24;
        // Status bar spacer
        var statusBarRow = new RowDefinition { Height = 24 };

        // WebView row
        var contentRow = new RowDefinition { Height = GridLength.Star };

        mainGrid.RowDefinitions.Add(statusBarRow);
        mainGrid.RowDefinitions.Add(contentRow);
        Grid.SetRow(boxView, 0);
        Grid.SetRow(WebViewElement, 1);
        mainGrid.Children.Add(boxView);
        mainGrid.Children.Add(WebViewElement);

        // this.Content=mainGrid;
        return mainGrid;

    }

    private View CreateOtherPlatformsLayout()
    {
        return WebViewElement;
    }
}


