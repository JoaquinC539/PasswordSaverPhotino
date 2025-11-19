using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PasswordSaver.Database;
using PasswordSaver.Models;
using SQLitePCL;

namespace PasswordSaver.Services;

public class ConfigService
{
    private static ConfigService? instance = null;
    // private readonly static ILogger logger = LoggerUtils.Factory.CreateLogger("ConfigService");

    private DB db = DB.GetDB();

    private ConfigService()
    {

    }
    public static ConfigService GetInstance()
    {
        if (instance == null)
        {
            instance = new ConfigService();
        }
        return instance;
    }
    public async Task<bool> ChangeDBInConfig()
    {
#if ANDROID
             return await ChangeDBInConfigAndroid();
            
#else
        return await ChangeDBInConfigDesktop();
#endif

    }

    public async Task<bool> ChangeDBInConfigAndroid()
    {


        var fileGet = await PickAndShow();
        if (fileGet == null)
        {
            Console.WriteLine("file is null");
            return false;
        }


        try
        {
            // Console.WriteLine("The file it was chosen: " + fileGet.FileName + " - " + fileGet.FullPath + " mime: " + fileGet.ContentType);
            if ( !fileGet.FileName.EndsWith(".db"))
            {
                return false;
            }
            using Stream pickedStream = await fileGet.OpenReadAsync();
            string internalFolder = FileSystem.AppDataDirectory;
            // Console.WriteLine($"Internal folder {internalFolder}");
            string destPath = Path.Combine(internalFolder, fileGet.FileName);
            // Console.WriteLine($"Dest path {destPath}");

            using FileStream destStream = File.Create(destPath);
            await pickedStream.CopyToAsync(destStream);
            // Console.WriteLine($"File copied to internal storage: {destPath}");
            string configFile = GetConfigFile();
            if (File.Exists(configFile))
            {
                // Console.WriteLine("Writing over config and updating db");
                // var newConfig = new { dbPath = FilePath };
                var newConfig = new { dbPath = destPath };
                var json = JsonSerializer.Serialize(newConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFile, json);
                db.ReStartDB();
                return true;
            }

            return false;

        }
        catch (IOException ex)
        {
            throw new PermissionDeniedException ($"Error writing database try resetting the phone and try again {ex.Message}");
        }
        catch (UnauthorizedAccessException e)
        {
            throw new PermissionDeniedException ($"Error writing database because access denied {e.Message}");
        }
        catch (System.Exception e)
        {

            throw new Exception($"Exception ocurred at ChangeDBInConfigAndroid {e.Message}");
        }
    }

    public async Task<bool> ChangeDBInConfigDesktop()
    {


        string configFile = GetConfigFile();
        // logger.LogInformation("ConfigFilePath: {path}", configFile);
        var fileGet = await PickAndShow();
        if (fileGet == null)
        {
            Console.WriteLine("file is null");
            return false;
        }
        try
        {
            string FilePath = fileGet.FullPath;
            // Console.WriteLine("The file it was chosen: "+fileGet.FileName + " - "+fileGet.FullPath+" mime: "+fileGet.ContentType);
            // Console.WriteLine("File exists? "+File.Exists(FilePath));
            // Console.WriteLine("File ends with .db: "+FilePath.EndsWith(".db")); 
            if (!File.Exists(FilePath) || !FilePath.EndsWith(".db"))
            {
                return false;
            }
            if (File.Exists(configFile))
            {
                // var newConfig = new { dbPath = FilePath };
                var newConfig = new { dbPath = fileGet.FullPath };
                var json = JsonSerializer.Serialize(newConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFile, json);
                db.ReStartDB();
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            throw new Exception("Error ocurred changing the databse (the database was not updated): " + e.Message);
        }
    }
    private string GetConfigFile()
    {
#if ANDROID || IOS
        string configFile = Path.Combine(FileSystem.AppDataDirectory, "psaverConfig.json");
#else
        string configFile = Path.Combine(AppContext.BaseDirectory, "psaverConfig.json");
#endif
        return configFile;

    }

    private bool CanReadFile(string filePath)
    {
        try
        {
            using var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
        catch (System.Exception e)
        {
            throw new Exception($"Exception ocurred at checking read file permission {e.Message}");
        }
    }

    private async Task<FileResult?> PickAndShow()
    {
        try
        {
            FilePickerFileType types = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    {DevicePlatform.WinUI, new[] { ".db"}},
                    {DevicePlatform.Android, new[] {"application/octet-stream", "application/x-sqlite3", "application/db"}}
                    
                }
            );
            PickOptions options = new()
            {
                PickerTitle = "Please select a database (.db) file",
                FileTypes = types
            };
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                return result;
            }
            return null;

        }
        catch (System.Exception e)
        {

            throw new Exception("Exception ocurred at filepicker " + e.Message);
        }
    }
}