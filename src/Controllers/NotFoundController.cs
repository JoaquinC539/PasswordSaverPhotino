
using PasswordSaver.Models;

namespace PasswordSaver.Controllers;

public class NotFoundController 
{
    public NotFoundController() { }

   

    public Task<object?> HandleAsyncTask(Request req)
    {
        var msg = new { Error = "Not Found type" };
        return Task.FromResult<object?>(msg);
        
    }
}