using AndroidX.DocumentFile.Provider;
using PasswordSaver.Database;
using PasswordSaver.Models;
using AndroidUri=Android.Net.Uri;
namespace PasswordSaver.Platforms.Android;


public class SafService : ISafService
{
    private DB DB = DB.GetDB();
    
    private DocumentFile? ResolveFolder (string uriString)
    {
        var context = Platform.AppContext!;
        AndroidUri treeUri = AndroidUri.Parse(uriString)!;
        return DocumentFile.FromTreeUri(context,treeUri);
    }

  public async Task<bool> BackUpToExternalFolderAsync(string uriString)
  {
    var context = Platform.AppContext!;
    var folder = ResolveFolder(uriString);
    if(folder == null)
    {
        return false;
    }
    string fileName =  $"CipheredBackup_{DateTime.Now.ToString("ddMMyy")}.db";
    var targetFile = folder.CreateFile("application/octet-stream",fileName);
    if( targetFile == null)
        {
            return false;
        }
    string tempPath = Path.Combine(FileSystem.CacheDirectory,"tempbackup.db");
    string sql = $"VACUUM main INTO '{tempPath.Replace("'", "''")}'";    
    DB.ExecQuery(sql);

    using var inStream = File.OpenRead(tempPath);
    using var outStream = context.ContentResolver!.OpenOutputStream(targetFile.Uri) ?? throw new PermissionDeniedException("Cannot open SAF output stream");

    await inStream.CopyToAsync(outStream);
    File.Delete(tempPath);
    return true;

  }

 
}