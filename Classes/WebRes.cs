namespace classes;

public interface WebRes
{
    public int Id { get; set; }
    public string Type { get; }

    public object  Payload { get; set; }
    
    
}