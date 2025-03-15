using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ManagePassword
{
	/*static*/
	internal class Cipher
	{
		private static byte[] salt;
		private static byte[] AESkey;
		private static byte[] AESIV;

		public byte[] Salt { get; set; }
		public byte[] AES_key { get; set; }
		public byte[] AESiv { get; set; }
		public byte[] CipherText { get; set; }
		public string DecryptedString { get; private set; }
		public Cipher(out byte[] _Salt, out byte[] AESKEY, out byte[] AES_iv, ref byte[] cipher, ref string str_cihper)
		{
			(Salt, AESiv, AES_key) = GenerateKeys();
			CipherText = EncryptAES(str_cihper, AES_key, AESiv);
			(_Salt, AES_iv, AESKEY, cipher) = (Salt, AESiv, AES_key, CipherText);
		}
		public Cipher(out string decrypt_str, out byte[] aesKey, ref byte[] salt, ref byte[] EncryptBytes,ref byte[] aesIV,ref string str)
		{
			aesKey = DeriveKey(str, salt);
			decrypt_str = DecryptAES(EncryptBytes, aesKey, aesIV);
		}

		public static (byte[], byte[], byte[]) GenerateKeys()
		{
			//byte[] Salt = null, iv = null, key = null;
			salt = GenerateSalt();
			AESIV = GenerateIV();
			AESkey = DeriveKey(AdmMode.AdmPassword, salt);
			return (salt, AESIV, AESkey);
		}
		public static void SetPass(string password, byte[] customSalt = null)
		{
			byte[] salt = null, AESkey = null, AESIV = null;
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
		public static byte[] EncryptAES(string password, byte[] key, byte[] iv)
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
			return encryptedBytes;
		}
		public static string DecryptAES(byte[] CipherPassowrd, byte[] key, byte[] iv)
		{
			Aes aes = Aes.Create();
			aes.Key = key;
			aes.IV = iv;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;

			ICryptoTransform decryptor = aes.CreateDecryptor();
			byte[] decryptedBytes = decryptor.TransformFinalBlock(CipherPassowrd, 0, CipherPassowrd.Length);
			return Encoding.UTF8.GetString(decryptedBytes);
		}
	}
}
