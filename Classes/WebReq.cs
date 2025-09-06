namespace classes;

public interface WebReq
{
    int Id { get; set; }
    string Type { get; }
    
    public string? Payload { get; set; }
}