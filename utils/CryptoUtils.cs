namespace utils;
using System;
using System.Security.Cryptography;
using System.Text;


public static class CryptoUtils
{
    public static string Encrypt(string plainText, string base64Key)
    {
        byte[] key = Convert.FromBase64String(base64Key);
        byte[] iv = RandomNumberGenerator.GetBytes(16);
        byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] ciphertext = new byte[plaintextBytes.Length];
        byte[] tag = new byte[16]; // GCM tag is always 16 bytes

        using (AesGcm aesGcm = new AesGcm(key))
        {
            aesGcm.Encrypt(iv, plaintextBytes, ciphertext, tag);
        }

        // Return format: iv:encrypted:tag (all hex)
        return $"{ByteArrayToHex(iv)}:{ByteArrayToHex(ciphertext)}:{ByteArrayToHex(tag)}";
    }

    public static string Decrypt(string encryptedText, string base64Key)
    {
        byte[] key = Convert.FromBase64String(base64Key);
        string[] parts = encryptedText.Split(':');
        if (parts.Length != 3)
            throw new FormatException("Invalid encrypted data format");

        byte[] iv = HexToByteArray(parts[0]);
        byte[] ciphertext = HexToByteArray(parts[1]);
        byte[] tag = HexToByteArray(parts[2]);
        byte[] plaintextBytes = new byte[ciphertext.Length];

        using (AesGcm aesGcm = new AesGcm(key))
        {
            aesGcm.Decrypt(iv, ciphertext, tag, plaintextBytes);
        }

        return Encoding.UTF8.GetString(plaintextBytes);
    }

    // Helper methods
    private static string ByteArrayToHex(byte[] bytes) =>
        BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();

    private static byte[] HexToByteArray(string hex)
    {
        int length = hex.Length;
        byte[] result = new byte[length / 2];
        for (int i = 0; i < result.Length; i++)
            result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        return result;
    }
}
