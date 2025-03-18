using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ManagePassword
{
	static internal class AdmMode
	{
		public static bool isAdm = false;
		public static string AdmPassword = "";

		public static NpgsqlCommand cmd = null;
		public static Cipher cipher;

		public static void GetAdmPassword(string AdmPassword_tb)
		{
			AdmPassword = AdmPassword_tb;
		}
		public static void RegistrAdm(string password, string admin = "Admin")
		{
			try
			{
				cipher = new Cipher(password);
				cipher.GenerateKeys();
				cipher.Hash_string = cipher.EncryptAES();

				if (Model.QueriesDB.BdMode == "Postgre")
				{
					NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO Admins(admin_name, password_hash, salt, aes_iv) VALUES(@username, @password_hash, @salt, @aes_iv)");

					cmd.Parameters.AddWithValue("@username", admin);
					cmd.Parameters.AddWithValue("@salt", cipher.Salt);
					cmd.Parameters.AddWithValue("@password_hash", cipher.Hash_string);
					cmd.Parameters.AddWithValue("@aes_iv", cipher.AESiv);
					PostgreSQL.single_query(cmd);
				}
				else if (Model.QueriesDB.BdMode == "SQLite")
				{

					SQLiteCommand if_cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Admins(id INTEGER PRIMARY KEY CHECK(id = 1), admin_name TEXT, password_hash BLOB NOT NULL, salt BLOB, aes_iv BLOB NOT NULL)");
					SQLite.single_query_for_SQLite(if_cmd);

					SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO Admins(id, admin_name, password_hash, salt, aes_iv) VALUES(@id, @username, @password_hash, @salt, @aes_iv)");
					cmd.Parameters.AddWithValue("@id", 1);
					cmd.Parameters.AddWithValue("@username", admin);
					cmd.Parameters.AddWithValue("@salt", cipher.Salt);
					cmd.Parameters.AddWithValue("@password_hash", cipher.Hash_string);
					cmd.Parameters.AddWithValue("@aes_iv", cipher.AESiv);

					SQLite.single_query_for_SQLite(cmd);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}
		public static bool AuthenticateAdm(string password)
		{
			try
			{
				cipher = new Cipher(password);
				if (Model.QueriesDB.BdMode == "Postgre")
				{
					(cipher.Salt, cipher.Hash_string, cipher.AESiv) = PostgreSQL.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'");
				}
				else if (Model.QueriesDB.BdMode == "SQLite")
				{
					(cipher.Salt,cipher.Hash_string,cipher.AESiv) = SQLite.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'");
				}
				if (cipher.Salt == null ||  cipher.Hash_string == null ||  cipher.AESiv == null)
					return false;
				byte[] encryptedPasswordBytes =  cipher.Hash_string;

				cipher.AES_key = cipher.DeriveKey(password, cipher.Salt);

				string decrypted_pass = cipher.DecryptAES(cipher.Hash_string, cipher.AES_key, cipher.AESiv);


				return decrypted_pass == password;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return false;
			}
		}
		public static void DeleteAdm(string password)
		{
			byte[] salt = null, pass_hash = null, aesIV = null;

			if (Model.QueriesDB.BdMode == "Postgre")
			{
				(salt, pass_hash, aesIV) = PostgreSQL.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'");
			}
			else if (Model.QueriesDB.BdMode == "SQLite")
			{
				(salt, pass_hash, aesIV) = SQLite.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'");
			}
			if (salt == null || pass_hash == null || aesIV == null)
				MessageBox.Show("User not found!");
			//TODO
			//byte[] aesKey = null;
			//string decrypted_pass;
			//byte[] aesKey = Cipher.DeriveKey(password, salt);
			//string decrypted_pass = Cipher.DecryptAES(pass_hash, aesKey, aesIV);

			//Cipher cipher = new Cipher(out decrypted_pass, out aesKey, ref salt, ref pass_hash, ref aesIV,ref password);

			//if (decrypted_pass == password)
			if(false)
			{
				string delteQuery = "DELETE FROM Admins WHERE admin_name = 'Admin' AND password_hash = @password_hash";
				if (Model.QueriesDB.BdMode == "Posetre")
				{
					NpgsqlCommand cmd = new NpgsqlCommand(delteQuery);
					cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Bytea, pass_hash);

					PostgreSQL.single_query(cmd);
					cmd.Dispose();
				}
				else if (Model.QueriesDB.BdMode == "SQLite")
				{
					SQLiteCommand cmd = new SQLiteCommand(delteQuery);
					cmd.Parameters.AddWithValue("@password_hash", pass_hash);

					SQLite.single_query_for_SQLite(cmd);
					cmd.Dispose();
				}
				AdmPassword = "";
				isAdm = false;		
			}
		}
		public static void ClearMasterPassword()
		{
			AdmPassword = "";
			isAdm = false;	
		}
	}
}
