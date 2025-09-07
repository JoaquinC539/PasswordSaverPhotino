using System.Text.Json;

namespace classes;

public class Request 
{
    public required string Type { get; set; }
    public int Id { get; set; }
    public JsonElement? Payload { get; set; }

    public Request() { }
    
}