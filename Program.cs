using classes;
using Photino.NET;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace HelloPhotinoApp
{
    //NOTE: To hide the console window, go to the project properties and change the Output Type to Windows Application.
    // Or edit the .csproj file and change the <OutputType> tag from "WinExe" to "Exe".
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Window title declared here for visibility
            MessageRouter messageRouter = new MessageRouter();
            string windowTitle = "Photino for .NET Demo App";
            // string pageUrl = "wwwroot/index.html";
            var pageUrl = new Uri("http://localhost:4200");

            // Creating a new PhotinoWindow instance with the fluent API
            var window = new PhotinoWindow()
                .SetTitle(windowTitle)
                // Resize to a percentage of the main monitor work area
                .SetUseOsDefaultSize(false)
                .SetSize(new Size(1024, 800))
                // Center window in the middle of the screen
                .Center()
                // Users can resize windows by default.
                // Let's make this one fixed instead.
                .SetResizable(false)
                
                // Most event handlers can be registered after the
                // PhotinoWindow was instantiated by calling a registration 
                // method like the following RegisterWebMessageReceivedHandler.
                // This could be added in the PhotinoWinsdowOptions if preferred.
                .RegisterWebMessageReceivedHandler((object sender, string message) =>
                {
                    var window = (PhotinoWindow)sender;
                    Console.WriteLine(message);
                    messageRouter.HandleRouter(window, message);
                    
                })
                
                .Load(pageUrl); // Can be used with relative path strings or "new URI()" instance to load a website.

            window.WaitForClose(); // Starts the application event loop
        }
    }
}
