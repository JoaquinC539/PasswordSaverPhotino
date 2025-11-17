namespace PasswordSaver.Models;

public class IncompleteDataException: Exception
{
    public IncompleteDataException(string message) : base(message)
    {

    }
    public IncompleteDataException(string message, Exception inner):base(message,inner)
    {
        
    }
}