
using PasswordSaver.Models;

namespace PasswordSaver.Controllers;

class GreetController 
{
    public GreetController() { }

    public Task<object?> HandleAsyncTask(Request req)
    {
        var gmsg = new
        {
            Message = "Hello from .NET",
            Test = new { TestText = "This is a nested text" }
        };

        return Task.FromResult<object?>(gmsg);
        
    }

    
}