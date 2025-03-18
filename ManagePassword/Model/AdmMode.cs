﻿using System;
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
	namespace Model
	{
		static internal class AdmMode
		{
			public static bool isAdm = false;
			public static string AdmPassword = "";

			public static NpgsqlCommand cmd = null;
			public static Model.Cipher cipher;

			public static void GetAdmPassword(string AdmPassword_tb)
			{
				AdmPassword = AdmPassword_tb;
			}
			public static void RegistrAdm(string password, string admin = "Admin")
			{
				try
				{
					cipher = new Model.Cipher(password);
					cipher.GenerateKeys(password);
					cipher.Hash_string = cipher.EncryptAES();

					if (Model.QueriesDB.BdMode == "Postgre")
					{
						NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO Admins(admin_name, password_hash, salt, aes_iv) VALUES(@username, @password_hash, @salt, @aes_iv)");

						cmd.Parameters.AddWithValue("@username", admin);
						cmd.Parameters.AddWithValue("@salt", cipher.Salt);
						cmd.Parameters.AddWithValue("@password_hash", cipher.Hash_string);
						cmd.Parameters.AddWithValue("@aes_iv", cipher.AESiv);
						Model.PostgreSQL.single_query(cmd);
					}
					else if (Model.QueriesDB.BdMode == "SQLite")
					{

						SQLiteCommand if_cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Admins(id INTEGER PRIMARY KEY CHECK(id = 1), admin_name TEXT, password_hash BLOB NOT NULL, salt BLOB, aes_iv BLOB NOT NULL)");
						Model.SQLite.single_query(if_cmd);

						SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO Admins(id, admin_name, password_hash, salt, aes_iv) VALUES(@id, @username, @password_hash, @salt, @aes_iv)");
						cmd.Parameters.AddWithValue("@id", 1);
						cmd.Parameters.AddWithValue("@username", admin);
						cmd.Parameters.AddWithValue("@salt", cipher.Salt);
						cmd.Parameters.AddWithValue("@password_hash", cipher.Hash_string);
						cmd.Parameters.AddWithValue("@aes_iv", cipher.AESiv);

						Model.SQLite.single_query(cmd);
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
					string decrypted_pass = "";
					if (Model.QueriesDB.BdMode == "Postgre")
					{
						decrypted_pass = Model.PostgreSQL.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'", password);
					}
					else if (Model.QueriesDB.BdMode == "SQLite")
					{
						decrypted_pass = Model.SQLite.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'", password);
					}
					if (cipher.Salt == null || cipher.Hash_string == null || cipher.AESiv == null)
						return false;
					
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
				string decrypted_pass = "";

				cipher = new Model.Cipher(password);

				if (Model.QueriesDB.BdMode == "Postgre")
				{
					decrypted_pass = Model.PostgreSQL.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'", password);
				}
				else if (Model.QueriesDB.BdMode == "SQLite")
				{
					decrypted_pass = Model.SQLite.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'", password);
				}
				if (cipher.Salt == null || cipher.Hash_string == null || cipher.AESiv == null)
					MessageBox.Show("User not found!");


				if (decrypted_pass == password)
				{
					string delteQuery = "DELETE FROM Admins WHERE admin_name = 'Admin' AND password_hash = @password_hash";
					if (Model.QueriesDB.BdMode == "Postgre")
					{
						NpgsqlCommand cmd = new NpgsqlCommand(delteQuery);
						cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Bytea, cipher.Hash_string);

						Model.PostgreSQL.single_query(cmd);
						cmd.Dispose();
					}
					else if (Model.QueriesDB.BdMode == "SQLite")
					{
						SQLiteCommand cmd = new SQLiteCommand(delteQuery);
						cmd.Parameters.AddWithValue("@password_hash", cipher.Hash_string);

						Model.SQLite.single_query(cmd);
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
}
