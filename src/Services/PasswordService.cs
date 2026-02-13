namespace PasswordSaver.Services;

using PasswordSaver.Database;
using PasswordSaver.Models;
using PasswordSaver.Utils;
public class PasswordService
{
    private static PasswordService? instance = null;

    private DB dB = DB.GetDB();

    private MasterPasswordService masterPasswordService;

    private PasswordService()
    {
        masterPasswordService = MasterPasswordService.GetInstance();
        if(masterPasswordService == null)
        {
            throw new NotImplementedException("masterPasswordService not initated");
        }
    }
    public static PasswordService GetInstance()
    {
        if (instance == null)
        {
            instance = new PasswordService();
        }
        return instance;
    }
     public async Task<string> SetAndGetEncryptKey()
    {
       
        string key = await masterPasswordService.GetEncryptKey();
        return key;
    }
    public async Task<List<Password>?> GetAllPasswordsAsync()
    {
        const string sqlScript = "SELECT * FROM passwords;";
        try
        {
            
            var rows = await dB.ExecQueryReturnAsync(sqlScript);
            if (rows == null) return null;
            var passwords = await GeneratePasswordsFromQuery(rows);
            return passwords;

        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine($"An error occurred getting passwords: {ex.Message}");
            return null;
        }
    }
    public async Task<List<Password>> GeneratePasswordsFromQuery(List<Dictionary<string, object>> rows)
    {
        string key= await SetAndGetEncryptKey();
        var result = new List<Password>();
        foreach (var row in rows)
        {
            var password = new Password()
            {
                Id = Convert.ToInt32(row["id"]),
                Name = row["name"].ToString()!,
                Username = row["username"].ToString()!,
                PasswordValue = CryptoUtils.Decrypt(row["password"].ToString()!, key),
                Notes = row.ContainsKey("notes") ? row["notes"]?.ToString() ?? "" : "",
                CreatedAt = row.ContainsKey("created_at") ? DateTime.Parse(row["created_at"].ToString()!) : DateTime.MinValue
            };
            result.Add(password);
        }
        return result;
    }
    public async Task<bool?> AddPasswordAsync(PasswordDto password)
    {
        try
        {
            string key = await SetAndGetEncryptKey();
            string encryptedPassword = CryptoUtils.Encrypt(password.PasswordValue, key);
            var columns = new List<string> { "name", "username", "password" };
            var placeholders = new List<string> { "@p0", "@p1", "@p2" };
            var paramenters = new List<object> { password.Name, password.Username, encryptedPassword };
            if (!string.IsNullOrEmpty(password.Notes))
            {
                columns.Add("notes");
                placeholders.Add("@p3");
                paramenters.Add(password.Notes!);
            }
            string sqlScript = $"INSERT INTO passwords ({string.Join(",", columns)}) VALUES ({string.Join(",", placeholders)});";
            bool inserted = dB.PreparedQuery(sqlScript, paramenters.ToArray());
            return inserted;

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error adding password: {ex.Message}");
            return null;
        }
    }
    public async Task<Password?> GetSinglePasswordByIdASync(int id)
    {
        string sqlScript = "SELECT * FROM passwords WHERE id = @p0";
        try
        {
            var row = await dB.PreparedQueryReturnAsync(sqlScript, id);
            if (row == null) return null;
            var passwordList = await GeneratePasswordsFromQuery(row);
            return passwordList[0];
        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine($"Error getting single password: {ex.Message}");
            throw;
        }
    }
    public async Task<bool?> EditPasswordAsync(PasswordDto password)
    {
        try
        {
            string key = await SetAndGetEncryptKey();
            string encryptedPassword = CryptoUtils.Encrypt(password.PasswordValue, key);
            var columns = new List<string> { "name", "username", "password" };
            var paramenters = new List<object> { password.Name, password.Username, encryptedPassword };
            if (!string.IsNullOrEmpty(password.Notes))
            {
                columns.Add("notes");
                paramenters.Add(password.Notes!);
            }
            string joiner = "";
            for (int i = 0; i < columns.Count; i++)
            {
                if (i == columns.Count - 1)
                {
                    joiner += $"{columns[i]}=@p{i}";
                }
                else
                {
                    joiner += $"{columns[i]}=@p{i}, ";
                }
            }

            string sqlScript = @$"
            UPDATE passwords
            SET {joiner}
            WHERE id=@p{columns.Count};            
            ";
            paramenters.Add(password.Id!);
            bool inserted = dB.PreparedQuery(sqlScript, paramenters.ToArray());
            return inserted;

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error adding password: {ex.Message}");
            return null;
        }
    }
    public Task<bool?> DeletePassword(int id)
    {
        try
        {
            string sqlScript = @"
            DELETE FROM passwords
            WHERE id=@p0";
            bool deleted = dB.PreparedQuery(sqlScript, id);
            return Task.FromResult((bool?)deleted);
        }
        catch (System.Exception ex)
        {

            Console.Error.WriteLine($"Error deleting password: {ex.Message}");
            return null;
            throw;
        }
    }
}