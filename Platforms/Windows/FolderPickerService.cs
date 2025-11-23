using PasswordSaver.Interfaces;
using Windows.Storage.Pickers;

namespace PasswordSaver.Platforms.Windows;

public class FolderPickerService : IFolderPicker
{
  public async Task<string?> PickFolderAsync()
  {
    var folderPicker = new FolderPicker();
    folderPicker.FileTypeFilter.Add("*");
    folderPicker.SuggestedStartLocation=PickerLocationId.DocumentsLibrary;
    if(App.Current == null)
        {
            return null;
        }
    var window = App.Current.Windows[0].Handler.PlatformView;
    IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
    WinRT.Interop.InitializeWithWindow.Initialize(folderPicker,hwnd);
    var result = await folderPicker.PickSingleFolderAsync();
    return result?.Path;

  }
}