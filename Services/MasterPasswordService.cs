using System.Security.Cryptography;
using System.Text;
using database;

namespace services;

public class MasterPasswordService
{
    private static MasterPasswordService? instance = null;
    private DB dB = DB.GetDB();

    private string encryptionKey = "";

    private MasterPasswordService() { }
    public static MasterPasswordService GetInstance()
    {
        if (instance == null)
        {
            instance = new MasterPasswordService();
        }
        return instance;
    }

    public async Task<int?> GetMasterPasswordCountAsync()
    {
        string sqlScript = "SELECT COUNT(*) as count FROM mainPassword;";
        var result = await dB.ExecQueryReturnAsync(sqlScript);
        if (result == null || result.Count == 0)
        {
            return 0;
        }

        return Convert.ToInt32(result[0]["count"]);

    }

    private string GenerateEncryptKey(string password, string salt)
    {
        // 600k iterations, 32-byte key, HMAC-SHA512
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            Encoding.UTF8.GetBytes(salt),
            600000,
            HashAlgorithmName.SHA512
        );

        byte[] keyBytes = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(keyBytes);
    }

    public string GetEncryptKey()
    {
        if (encryptionKey == "")
        {
            throw new InvalidOperationException("Encryption key not initialized. Did you log in?");
        }
        return encryptionKey;
    }

    public Task<bool?> InsertMasterPasswordAsync(string password)
    {
        try
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(32);
            string keySalt = Convert.ToBase64String(saltBytes);

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
            string sql = "INSERT INTO mainPassword (password_hash, key_salt) VALUES (@p0, @p1);";
            bool inserted = dB.PreparedQuery(sql, hashPassword, keySalt);

            encryptionKey = GenerateEncryptKey(password, keySalt);

            return Task.FromResult((bool?)inserted);
        }
        catch (System.Exception ex)
        {
            Console.Error.WriteLine("Error inserting master password: " + ex.Message);
            return null;
        }
    }
    public async Task<bool?> MakeLoginComparisonAsync(string password)
    {
        string sql = "SELECT * FROM mainPassword;";
        try
        {
            var res = await dB.ExecQueryReturnAsync(sql);
            if (res == null || res.Count == 0) return null;

            string hashPassword = res[0]["password_hash"]?.ToString();
            string keySalt = res[0]["key_salt"]?.ToString();

            bool samePassword = BCrypt.Net.BCrypt.Verify(password, hashPassword);
            if (samePassword)
            {
                encryptionKey = GenerateEncryptKey(password, keySalt);
            }
            return samePassword;
        }
        catch (System.Exception)
        {

            return null;
        }
    }

    public void MakeLogoutAsync()
    {
        encryptionKey = "";
    }

}