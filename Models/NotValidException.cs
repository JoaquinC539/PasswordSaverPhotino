namespace PasswordSaver.Models;

public class NotValidException: Exception
{
    public NotValidException(string message):base(message)
    {
        

    }
    public NotValidException(string message, Exception inner):base(message,inner)
    {
        

    }
}