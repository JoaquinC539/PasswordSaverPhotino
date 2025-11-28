namespace PasswordSaver.Interfaces;

public interface IFolderPicker
{
    /// <summary>
    /// Open the picker and return the URI of the tree as string (ej: content://com.android...)
    /// Returns null if user cancels.
    /// </summary>
    Task<string?> PickFolderAsync();
}