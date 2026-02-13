using Gtk;
using PasswordSaver.Models;

public class Pickers
{
    public static async Task<string> PickFileAsyncGtk4(string title,string[]? patterns ,Gtk.Window? parent)
    {
        // var tcs = new TaskCompletionSource<string?>();
        var dialog = FileDialog.New();
        dialog.Title = title;

        var filter = FileFilter.New();
        filter.Name = "Database files (*.db)";
        if(patterns == null)
        {
            patterns = ["*.db"];
        }
        
        foreach (var p in patterns)
        {
            filter.AddPattern(p);
        } 
        dialog.SetDefaultFilter(filter);
        var file= await dialog.OpenAsync(parent);
        if(file == null)
        {
            throw new TaskCanceledException();
        }
        
        string filePath=file!.GetPath();
        if(!filePath!.EndsWith(".db")) throw new NotValidException("Not valid file");
        
        return filePath;
    }

    public static async Task<string> PickFolderAsyncGtk4(string title, Window? parent)
    {
        var dialog = FileDialog.New();
        dialog.Title=title;
        var dir=await dialog.SelectFolderAsync(parent);
        if(dir == null)
        {
            throw new TaskCanceledException();
        }
        return dir!.GetPath();
        
    }
}