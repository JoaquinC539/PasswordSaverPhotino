namespace controllers;
using System.Text.Json;
using classes;

public class NotFoundController: IController 
{
    public NotFoundController() { }

   

    public Task<object?> HandleAsyncTask(Request req)
    {
        var msg = new { Error = "Not Found type" };
        return Task.FromResult<object?>(msg);
        
    }
}