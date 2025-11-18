using System.Text.Json;

using Microsoft.Extensions.Logging;
using PasswordSaver.Database;

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
    public bool ChangeDBInConfig(string FilePath)
    {
        try
        {
            string configFile = GetConfigFile();
            // logger.LogInformation("ConfigFilePath: {path}", configFile);
            if ( !File.Exists(FilePath) || !FilePath.EndsWith(".db"))
            {
                return false;
            }
            if (File.Exists(configFile))
            {
                var newConfig = new { dbPath = FilePath };
                var json = JsonSerializer.Serialize(newConfig, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFile, json);
                db.ReStartDB();
                return true;
            }

            return false;
        }
        catch (Exception e)
        {
            throw new Exception("Error ocurred changing the databse (the database was not updated): "+e.Message);
            // logger.LogError("Error ocurred: {e}", e.Message);
            return false;
        }

    }

    private string GetConfigFile()
    {
#if ANDROID || IOS
        string configFile = Path.Combine(FileSystem.AppDataDirectory,"psaverConfig.json");
#else
        string configFile = Path.Combine(AppContext.BaseDirectory, "psaverConfig.json");
#endif
        return configFile;

    }
}