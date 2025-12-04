using System.Text.Json;
using Microsoft.Data.Sqlite;


namespace PasswordSaver.Database;

public class DB
{
    private static DB? dbInstance;

    private SqliteConnection? connection;

    public string DbPath;

    private DB()
    {
        DbPath = GetConfigJson();
        Console.WriteLine(DbPath);
        CheckOrCreateDB();
    }

    public static DB GetDB()
    {
        return dbInstance ??= new DB();
    }

    private string GetConfigFile()
    {
#if ANDROID || IOS
        string configFile = Path.Combine(FileSystem.AppDataDirectory, "psaverConfig.json");
#else
        // string configFile = Path.Combine(AppContext.BaseDirectory, "psaverConfig.json");
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PasswordSaver"));
        string configFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PasswordSaver","psaverConfig.json");
#endif
        
        Console.WriteLine($"path to create files {configFile}");
        return configFile;

    }

    private string GetDefaultDbPath()
    {
#if ANDROID || IOS
        string defaultDBPath = Path.Combine(FileSystem.AppDataDirectory, "data.db");
#else
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PasswordSaver"));
        string defaultDBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PasswordSaver", "data.db");
#endif
        Console.WriteLine($"path for standard db {defaultDBPath}");
        return defaultDBPath;

    }

    private string GetConfigJson()
    {
        try
        {
            string configFile = GetConfigFile();
            if (!File.Exists(configFile))
            {
                string defaultDBPath = GetDefaultDbPath();
                var config = new { dbPath = defaultDBPath };
                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configFile, json);
                Console.WriteLine("Config JSON created at: " + configFile);
                Console.WriteLine("Created this dbpath: " + defaultDBPath);
                return defaultDBPath;
            }
            else
            {
                string json = File.ReadAllText(configFile);
                using var doc = JsonDocument.Parse(json);
                string path = doc.RootElement.GetProperty("dbPath").GetString()!;
                Console.WriteLine("Using this path to DB" + path);
                return path;
            }
        }
        catch (System.Exception e)
        {
            setToDefaultConfig();
            throw new Exception($"An exception ocurred at setting configs, resetting to default: {e.Message}");
        }


    }
    private void setToDefaultConfig()
    {
        string configFile = GetConfigFile();
        Console.WriteLine($"Reset string path {configFile}");
        if (File.Exists(configFile))
        {
            Console.WriteLine("reseting default db values");
            var newConfig = new { dbPath = GetDefaultDbPath() };
            var json = JsonSerializer.Serialize(newConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFile, json);
        }
        
    }
    public void CheckOrCreateDB()
    {

        if (!File.Exists(DbPath))
        {
            File.Create(DbPath).Close();
            Console.WriteLine("DB file created at: " + DbPath);
        }

        ConnectDb();
    }
    public void ConnectDb()
    {
        try
        {   
            Console.WriteLine($"Trying to connect to {DbPath}");
            string connectionString = $"Data Source={DbPath}";
            connection = new SqliteConnection(connectionString);
            connection.Open();
            Console.WriteLine("SQLite connection successful");
        }
        catch (System.Exception e)
        {
            setToDefaultConfig();
            throw new Exception("There was an exception at opening sqlite connection: " + e.Message);
        }

    }
    
    public async Task DisonnectAndDisposeconnection()
    {
        if(connection != null)
        {
            connection.Close();
            await connection.DisposeAsync();
        
        }
    }
    
    public void ReStartDB()
    {
        try
        {
            connection?.Close();
            DbPath = GetConfigJson();
            CheckOrCreateDB();
            CreateOrCheckTables();
            // using var sha = System.Security.Cryptography.SHA256.Create();
        }
        catch (System.Exception e)
        {
            setToDefaultConfig();
            ReStartDB();
            throw new Exception("Exception at restarting DB: " + e.Message);
        }



    }
    

    public void CreateOrCheckTables()
    {

        string masterTableSql = @"
        CREATE TABLE IF NOT EXISTS mainPassword(
               id INTEGER PRIMARY KEY CHECK (id=1),
               password_hash TEXT NOT NULL,
                key_salt TEXT NOT NULL,      
               enforce_single_row INTEGER DEFAULT 1 CHECK (enforce_single_row=1)
            );
        ";
        string passwordsTableSql = @"
        CREATE TABLE IF NOT EXISTS passwords (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            username TEXT NOT NULL,
            password TEXT NOT NULL,
            notes TEXT,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP
        );
    ";
        try
        {
            using var cmd1 = connection.CreateCommand();
            cmd1.CommandText = masterTableSql;
            cmd1.ExecuteNonQuery();


            using var cmd2 = connection.CreateCommand();
            cmd2.CommandText = passwordsTableSql;
            cmd2.ExecuteNonQuery();
            Console.WriteLine("Tables created or checked successfully");
        }
        catch (System.Exception e)
        {

            throw new Exception($"An exception ocurred " );
        }

    }
    public bool ExecQuery(string sql)
    {
        if (connection == null)
        {
            return false;
        }
        try
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (System.Exception err)
        {
            throw new Exception($"Error ocurred exeuting query: {err.Message}");
        }
    }

    public async Task<List<Dictionary<string, object>>> ExecQueryReturnAsync(string sql)
    {
        try
        {
            var results = new List<Dictionary<string, object>>();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                results.Add(row);
            }
            return results;

        }
        catch (Exception ex)
        {
            throw new Exception("Error at running fetch SQL: " + ex.Message);

        }
    }

    public bool PreparedQuery(string sql, params object[] values)
    {
        try
        {
            if (values.Length == 0)
            {
                return false;
            }
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            for (int i = 0; i < values.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@p{i}", values[i]);
            }
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (System.Exception ex)
        {

            throw new Exception("Error occurred doing prep query: " + ex.Message);
        }
    }
    public async Task<List<Dictionary<string, object>>?> PreparedQueryReturnAsync(string sql, params object[] values)
    {
        try
        {
            if (values.Length == 0) return null;

            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            for (int i = 0; i < values.Length; i++)
            {
                cmd.Parameters.AddWithValue($"@p{i}", values[i]);
            }
            var results = new List<Dictionary<string, object>>();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                results.Add(row);
            }
            return results;
        }
        catch (System.Exception ex)
        {
            throw new Exception("Error occurred doinga prep fetch query: " + ex.Message);
        }
    }


}