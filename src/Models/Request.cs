
using System.Text.Json;

namespace PasswordSaver.Models;

public class Request 
{
    public required string Type { get; set; }
    public int Id { get; set; }
    public JsonElement? Payload { get; set; }

    public Request() { }
    
}