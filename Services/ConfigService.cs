using System.Text.Json;
using database;
using Microsoft.Extensions.Logging;
using utils;

namespace services;

public class ConfigService
{
    private static ConfigService? instance = null;
    private readonly static ILogger logger = LoggerUtils.Factory.CreateLogger("ConfigService");

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
            string configFile = Path.Combine(AppContext.BaseDirectory, "psaverConfig.json");
            logger.LogInformation("ConfigFilePath: {path}", configFile);
            if (!FilePath.EndsWith(".db"))
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
            logger.LogError("Error ocurred: {e}", e.Message);
            return false;
        }

    }
}