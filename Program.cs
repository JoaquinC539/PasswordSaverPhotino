



Console.WriteLine("Hello World");
   bool IsLinux = OperatingSystem.IsLinux();
if (!IsLinux)
{
    Console.WriteLine("Not suppoerted platform, only linux");
    return ;
} 

try
{
    WebKit.Module.Initialize();
}
catch (System.Exception)
{
    
    Console.WriteLine("An exception occurred it might have to be because it requires webkit 6");
    Console.WriteLine("Please run sudo apt install libwebkitgtk-6.0-dev and try again");
    throw;
}
    

