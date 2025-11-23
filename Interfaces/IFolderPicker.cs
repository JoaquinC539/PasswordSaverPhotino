namespace PasswordSaver.Interfaces;

public interface IFolderPicker
{
    Task<string?> PickFolderAsync();
}