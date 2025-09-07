using classes;

public interface IController
{
    public Task<object?> HandleAsyncTask(Request req);
}