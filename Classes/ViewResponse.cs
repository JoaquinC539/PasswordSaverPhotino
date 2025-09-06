using classes;

public class ViewResponse : WebRes
{
    public int Id { get; set; }
    public string Type { get; set; }

    public object Payload { get; set; }

    public ViewResponse() { }
    public ViewResponse(int id, string type, object payload)
    {
        Id = id;
        Type = type;
        Payload = payload;
    }
}