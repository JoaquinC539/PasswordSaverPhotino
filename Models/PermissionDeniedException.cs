namespace PasswordSaver.Models;

public class PermissionDeniedException:Exception
{
    public PermissionDeniedException(string message):base(message)
    {
        
    }
    public PermissionDeniedException(string message,Exception inner):base(message,inner)
    {
        
    }
}