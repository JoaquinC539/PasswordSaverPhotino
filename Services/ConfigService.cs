using System.Text.Json;
using PasswordSaver.Database;
using PasswordSaver.Utils;

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
        var patterns= new[] {".db","*.db","*.DB",".DB"};
        var filePath =await  Pickers.PickFileAsyncGtk4("Select a DB to import",patterns,null);
        if(!File.Exists(filePath) || !filePath.EndsWith(".db")) return false;
        string fileName=Path.GetFileName(filePath); 
        string localDataPath= Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string newDbPath=Path.Combine(localDataPath,"PasswordSaver",fileName);        
        var dbconfig =JsonSerializer.Serialize(new {dbPath=newDbPath}, new JsonSerializerOptions {WriteIndented = true});  
        string configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PasswordSaver","psaverConfig.json");
        Copier.Copy(filePath,newDbPath);
        Copier.WriteOver(configFile,dbconfig);
        db.ReStartDB();
        return true;
    }
}