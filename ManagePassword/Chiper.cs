using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ManagePassword
{
    static internal class Chiper
    {
        private static readonly byte[] aesKey = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("YourSecurePassowrdKey"));
        private static readonly byte[] aesIV = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("YourIVKey"));

        static string EncryptAES(string password)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] plainBytes = Encoding.UTF8.GetBytes(password);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }
        static string DecryptAES(string CipherPassowrd)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key,aes.IV);
                byte[] cipherBytes = Convert.FromBase64String(CipherPassowrd);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
