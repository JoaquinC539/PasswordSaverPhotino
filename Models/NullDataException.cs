namespace PasswordSaver.Models;

public class NullDataException: Exception
{
    public NullDataException(string message) : base(message)
    {

    }
    public NullDataException(string message, Exception inner):base(message,inner)
    {
        
    }
}