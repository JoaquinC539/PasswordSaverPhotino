using System.Text.Json;
using Microsoft.Data.Sqlite;

namespace database;

public class DB
{
    private static DB dbInstance;
    private SqliteConnection connection;
    private string DbPath;

    private DB()
    {
        DbPath = GetConfigJson();
        Console.WriteLine(DbPath);
    }

    public static DB GetDB()
    {
        return dbInstance ??= new DB();
    }

    private string GetConfigJson()
    {
        string configFile = Path.Combine(AppContext.BaseDirectory, "psaverConfig.json");
        if (!File.Exists(configFile))
        {
            string defaultDBPath = Path.Combine(AppContext.BaseDirectory, "data.db");
            var config = new { dbPath = defaultDBPath };
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configFile, json);
            Console.WriteLine("Config JSON created at: " + configFile);
            return defaultDBPath;
        }
        else
        {
            string json = File.ReadAllText(configFile);
            using var doc = JsonDocument.Parse(json);
            string path = doc.RootElement.GetProperty("dbPath").GetString()!;
            return path;
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
        string connectionString = $"Data Source={DbPath}";
        connection = new SqliteConnection(connectionString);
        connection.Open();

    }
    public void ReStartDB()
    {
        connection.Close();
        DbPath=GetConfigJson();       
        CheckOrCreateDB();
        CreateOrCheckTables();
        using var sha = System.Security.Cryptography.SHA256.Create();
        

    }
    public SqliteConnection GetConnection() => connection;

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
        using var cmd1 = connection.CreateCommand();
        cmd1.CommandText = masterTableSql;
        cmd1.ExecuteNonQuery();


        using var cmd2 = connection.CreateCommand();
        cmd2.CommandText = passwordsTableSql;
        cmd2.ExecuteNonQuery();
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
            Console.WriteLine($"Error ocurred exeuting query: {err.Message}");
            return false;
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
            Console.Error.WriteLine("Error at running SQL: " + ex.Message);
            return null;
            
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

            Console.Error.WriteLine("Error occurred doing query: " + ex.Message);
            return false;
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
            Console.Error.WriteLine("Error occurred doing query: " + ex.Message);
            return null;
        }
    }
}