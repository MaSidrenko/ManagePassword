using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Npgsql;

namespace ManagePassword
{
	namespace Model
	{
		static internal class SQLite
		{
			const string CONN_STR = "Data Source = PassData.db;Version = 3";
			//Используется для защиты от SQL-инъекций
			static public Dictionary<string, object> Create_and_set_parameters(SQLiteCommand cmd, string open_string, byte[] cipher_password, byte[] salt, byte[] iv)
			{
				Dictionary<string, object> param = new Dictionary<string, object>
				{
					{ "@open_string", open_string },
					{ "@password_hash", cipher_password },
					{ "@salt", salt },
					{ "@aes_iv", iv }
				};

				foreach (KeyValuePair<string, object> prm in param)
				{
					cmd.Parameters.AddWithValue(prm.Key, prm.Value);
				}
				return param;
			}
			//Получение всех данных из БД
			static public DataTable Refresh()
			{
				DataTable dataTable = new DataTable();
				SQLiteCommand temp_cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS PasswordCihper(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, open_string TEXT, password_hash BLOB NOT NULL, salt BLOB NOT NULL, aes_iv BLOB NOT NULL);");
				single_query(temp_cmd);
				if (AdmMode.isAdm)
				{
					temp_cmd = new SQLiteCommand("SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper");
					dataTable = circle_query(temp_cmd);
					return dataTable;
				}
				else
				{
					temp_cmd = new SQLiteCommand("SELECT id AS \"Number\", open_string AS \"Service\" FROM PasswordCihper");
					dataTable = circle_query(temp_cmd);
					return dataTable;
				}
			}
			static public void Insert(string Service, string Password)
			{
				try
				{
					Cipher cipher = new Cipher(Password);
					cipher.GenerateKeys();
					cipher.Encrypt();

					SQLiteCommand temp_cmd = new SQLiteCommand("INSERT INTO PasswordCihper(open_string, password_hash, salt, aes_iv) VALUES(@open_string, @password_hash, @salt, @aes_iv)");
					Dictionary<string, object> parameters = Create_and_set_parameters(temp_cmd, Service, cipher.Hash_string, cipher.Salt, cipher.AESiv);
					single_query(temp_cmd, Service, Password);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			static public DataTable Find(string Service)
			{
				DataTable dataTable = new DataTable();
				if (AdmMode.isAdm)
				{
					SQLiteCommand temp_cmd = new SQLiteCommand("SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper WHERE open_string LIKE @open");
					temp_cmd.Parameters.AddWithValue("@open", Service + "%");
					dataTable = circle_query(temp_cmd);
					return dataTable;
				}
				else if (!AdmMode.isAdm)
				{
					SQLiteCommand temp_cmd = new SQLiteCommand("SELECT id AS \"Nubmer\", open_string AS \"Service\" FROM PasswordCihper WHERE open_string LIKE @open");
					temp_cmd.Parameters.AddWithValue("@open", Service + "%");
					dataTable = circle_query(temp_cmd);
					return dataTable;
				}
				else
				{
					return Refresh();
				}
			}
			static public void Delete(string delItem)
			{
				SQLiteCommand temp_cmd = new SQLiteCommand("DELETE FROM PasswordCihper WHERE id = @id");
				temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(delItem));
				single_query(temp_cmd);
			}
			static public void Change(string id, string Service, string Password)
			{
				if (AdmMode.isAdm)
				{
					Cipher cipher = new Cipher(Password);
					cipher.GenerateKeys();
					cipher.Encrypt();

					SQLiteCommand temp_cmd = new SQLiteCommand("UPDATE PasswordCihper SET open_string = @open_string, password_hash = @password_hash, salt = @salt, aes_iv = @aes_iv WHERE id = @id");
					Dictionary<string, object> parameters = Create_and_set_parameters(temp_cmd, Service, cipher.Hash_string, cipher.Salt, cipher.AESiv);
					temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));

					single_query(temp_cmd, Service, Password);
				}
			}
			static public DataTable circle_query(SQLiteCommand cmd)
			{
				DataTable dataTable = new DataTable();
				SQLiteConnection conn = new SQLiteConnection(CONN_STR);
				SQLiteDataReader reader = null;
				try
				{
					conn.Open();
					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					reader = cmd.ExecuteReader();
					dataTable.Load(reader);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				finally
				{
					if (reader != null && !reader.IsClosed)
					{
						reader.Close();
					}
					cmd.Dispose();
					conn.Close();
					conn.Dispose();
				}
				return dataTable;
			}
			static public void single_query(SQLiteCommand cmd, string Service = "", string Password = "")
			{
				try
				{
					SQLiteConnection conn = new SQLiteConnection(CONN_STR);
					conn.Open();

					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.ExecuteNonQuery();

					cmd.Dispose();
					conn.Close();
					conn.Dispose();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			static public string read_adm_password(string query, string password)
			{
				Cipher cipher = new Cipher(password);
				SQLiteConnection conn_DB = new SQLiteConnection(CONN_STR);
				SQLiteCommand cmd = new SQLiteCommand(query, conn_DB);
				conn_DB.Open();

				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					cipher.Hash_string = (byte[])reader["password_hash"];
					cipher.Salt = (byte[])reader["salt"];
					cipher.AESiv = (byte[])reader["aes_iv"];
				}
				cipher.AES_key = cipher.DeriveKey(password, cipher.Salt);
				reader.Close();
				cmd.Dispose();
				conn_DB.Close();
				conn_DB.Dispose();
				return cipher.Decrypt(cipher.Hash_string, cipher.AES_key, cipher.AESiv);
			}
			static public void create_adm_password(Cipher cipher, string admin)
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
			//Старнно написанный метод
			//Используется для получения только для получения дешифрованного солбца пароля
			static public DataTable read_passwords(DataTable table)
			{
				string decrypted = "";
				Model.Cipher cihper = new Model.Cipher(AdmMode.UnsaveGetAdmPassword());
				foreach (DataRow row in table.Rows)
				{
					cihper.Hash_string = (byte[])row["password_hash"];
					cihper.Salt = (byte[])row["salt"];
					cihper.AESiv = (byte[])row["aes_iv"];


					if (cihper.Hash_string != null && cihper.Salt != null && cihper.AESiv != null)
					{
						cihper.AES_key = cihper.DeriveKey(cihper.Input_string, cihper.Salt);
						decrypted = cihper.Decrypt(cihper.Hash_string, cihper.AES_key, cihper.AESiv);
						row["password"] = decrypted;
					}
				}
				return table;
			}
		}
	}
}