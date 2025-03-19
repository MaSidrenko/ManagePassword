using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ManagePassword
{
	namespace Model
	{
		internal class Cipher
		{
			public byte[] Salt { get; set; }
			public byte[] AES_key { get; set; }
			public byte[] AESiv { get; set; }
			public byte[] Hash_string { get; set; }
			public string DecryptedString { get; private set; }
			public string Input_string { get; set; }

			public Cipher(string input_string)
			{
				Input_string = input_string;
			}
			/*public void SetPass(string password)
			{
				Salt = GenerateSalt();
				AESiv = GenerateIV();
				AES_key = DeriveKey(password, Salt);
			}*/

			public (byte[], byte[], byte[]) GenerateKeys(string password = "")
			{
				//salt - random;
				Salt = GenerateSalt();
				//AESIV - random;
				AESiv = GenerateIV();
				//AESkey - not random. Based on Master-Password;
				AES_key = DeriveKey(password != "" ? password : AdmMode.AdmPassword, Salt);
				return (Salt, AESiv, AES_key);
			}
			public byte[] DeriveKey(string password, byte[] salt, int keySize = 32, int iterations = 10000)
			{
				Rfc2898DeriveBytes pdkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
				byte[] key = pdkdf2.GetBytes(keySize);
				pdkdf2.Dispose();
				return key;
			}
			public byte[] GenerateSalt()
			{
				byte[] salt = new byte[16];
				RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
				rng.GetBytes(salt);
				rng.Dispose();
				return salt;
			}
			public byte[] GenerateIV()
			{
				byte[] iv = new byte[16];
				RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
				rng.GetBytes(iv);
				rng.Dispose();
				return iv;
			}
			public void Encrypt()
			{
				Aes aes = Aes.Create();
				aes.Key = AES_key;
				aes.IV = AESiv;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				ICryptoTransform encryptor = aes.CreateEncryptor(AES_key, AESiv);
				byte[] inputBytes = Encoding.UTF8.GetBytes(Input_string);
				byte[] encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
				encryptor.Dispose();
				Hash_string = encryptedBytes;
			}
			public string Decrypt(byte[] CipherPassowrd, byte[] key, byte[] iv)
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
}
