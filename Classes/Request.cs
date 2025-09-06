namespace classes;

public class Request : WebReq
{
    public required string Type { get; set; }
    public int Id { get; set; }
    public string? Payload { get; set; }

    public Request() { }
    public Request(string type, int id)
    {
        Id = id;
        Type = type;
    }
    public Request(string type, int id, string payload)
    {
        Id = id;
        Type = type;
        Payload = payload;
    }
}