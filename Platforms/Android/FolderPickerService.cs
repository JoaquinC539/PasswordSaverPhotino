
using Android.App;
using Android.Content;
using Android.Runtime;
using PasswordSaver.Interfaces;

namespace PasswordSaver.Platforms.Android;

public class FolderPickerService : IFolderPicker
{
    TaskCompletionSource<string?>? _tcs;

    public static Action<Result,Intent?>? ActivityResultCallback {get; set;}
    public Task<string?> PickFolderAsync()
    {
        _tcs = new TaskCompletionSource<string?>();
        var activity = Platform.CurrentActivity ?? throw new Exception("No current activity");
        var intent = new Intent(Intent.ActionOpenDocumentTree);
        intent.AddFlags(
            ActivityFlags.GrantPrefixUriPermission |
            ActivityFlags.GrantPrefixUriPermission |
            ActivityFlags.GrantReadUriPermission |
            ActivityFlags.GrantWriteUriPermission
        );
        ActivityResultCallback = OnActivityResult;
        activity.StartActivityForResult(intent,9999);
        return _tcs.Task;
    }

    private void OnActivityResult(Result resultCode, Intent? data)
    {
        Console.WriteLine("Called callback OnActivityResult");
        if(_tcs == null)
        {
            return;
        }
        if(resultCode == Result.Canceled)
        {
            _tcs.TrySetCanceled();
            return ;
        }
        Console.WriteLine("tcs not null");
        if(resultCode == Result.Ok && data?.Data != null)
        {
            var uri = data.Data;
            var activity = Platform.CurrentActivity!;
            activity.ContentResolver.TakePersistableUriPermission(uri,
            ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);   
            _tcs.TrySetResult(uri.ToString());
        }
       
    }

    

   

    
}