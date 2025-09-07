using classes;

public class ViewResponse : WebRes
{
    public int Id { get; set; }
    public string Type { get; set; } = default!;
    public object? Payload { get; set; }
    public bool Success { get; set; } = true;
    public string? Error { get; set; }
}