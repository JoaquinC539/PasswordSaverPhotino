using System.Security.Cryptography;
using System.Text;
using database;

namespace services;

public class MasterPasswordService
{
    private static MasterPasswordService instance = null;
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
}