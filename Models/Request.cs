
namespace PasswordSaver.Models;

public class Request 
{
    public required string Type { get; set; }
    public int Id { get; set; }
    public object? Payload { get; set; }

    public Request() { }
    
}