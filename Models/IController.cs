namespace PasswordSaver.Models;

public interface IController
{
    public Task<object?> HandleAsyncTask(Request req);
}