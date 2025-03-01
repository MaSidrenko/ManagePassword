using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ManagePassword
{
    static internal class Cipher
    {
        private static byte[] salt;
        private static byte[] AESkey;
        private static byte[] AESIV;

        public static void SetPass(string password, byte[] customSalt = null)
        {
            salt = customSalt ?? GenerateSalt();
            AESkey = DeriveKey(password, salt);
            AESIV = GenerateIV();
        }
        public static byte[] DeriveKey(string password, byte[] salt, int keySize = 32, int iterations = 10000)
        {
            Rfc2898DeriveBytes pdkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] key = pdkdf2.GetBytes(keySize);
            pdkdf2.Dispose();
            return key;
        }
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            rng.Dispose();
            return salt;
        }
        public static byte[] GenerateIV()
        {
            byte[] iv = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(iv);
            rng.Dispose();
            return iv;
        }
        public static string EncryptAES(string password, byte[] key, byte[] iv)
        {
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            encryptor.Dispose();
            return Convert.ToBase64String(encryptedBytes);  
        }
        public static string DecryptAES(string CipherPassowrd, byte[] key, byte[] iv)
        {
            Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;   
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(CipherPassowrd);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            decryptor.Dispose();
            aes.Dispose();
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
