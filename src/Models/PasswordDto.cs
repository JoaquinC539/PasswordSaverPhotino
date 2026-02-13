namespace PasswordSaver.Models;
public class PasswordDto
{

    public int? Id { get; set; }
    public string Name { get; set; } = "";
    public string Username { get; set; } = "";
    public string PasswordValue { get; set; } = "";
    public string? Notes { get; set; }
}