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
			static string conn_str = "Data Source = PassData.db; Version = 3";
			static SQLiteCommand temp_cmd;
			static Cipher cipher;
			static public DataTable Refresh()
			{

				DataTable dataTable = new DataTable();
				temp_cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS PasswordCihper(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, open_string TEXT, password_hash BLOB NOT NULL, salt BLOB NOT NULL, aes_iv BLOB NOT NULL);");
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

					cipher = new Cipher(Password);
					cipher.GenerateKeys();
					cipher.Hash_string = cipher.EncryptAES();


					temp_cmd = new SQLiteCommand("INSERT INTO PasswordCihper(open_string, password_hash, salt, aes_iv) VALUES(@open_string, @password_hash, @salt, @aes_iv)");
					Dictionary<string, object> parameters = Create_and_set_parameters(temp_cmd, Service, cipher.Hash_string, cipher.Salt, cipher.AESiv);
					single_query(temp_cmd);
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
					temp_cmd = new SQLiteCommand("SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper WHERE open_string LIKE @open");
					temp_cmd.Parameters.AddWithValue("@open", Service + "%");
					dataTable = circle_query(temp_cmd);
					return dataTable;
				}
				else if (!AdmMode.isAdm)
				{
					temp_cmd = new SQLiteCommand("SELECT id AS \"Nubmer\", open_string AS \"Service\" FROM PasswordCihper WHERE open_string LIKE @open");
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
				temp_cmd = new SQLiteCommand("DELETE FROM PasswordCihper WHERE id = @id");
				temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(delItem));
				single_query(temp_cmd);
			}
			static public void Change(string id, string Service, string Password)
			{
				if (AdmMode.isAdm)
				{

					cipher = new Cipher(Password);
					cipher.GenerateKeys();
					cipher.Hash_string = cipher.EncryptAES();

					temp_cmd = new SQLiteCommand("UPDATE PasswordCihper SET open_string = @open_string, password_hash = @password_hash, salt = @salt, aes_iv = @aes_iv WHERE id = @id");
					Dictionary<string, object> parametres = Create_and_set_parameters(temp_cmd, Service, cipher.Hash_string, cipher.Salt, cipher.AESiv);
					temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));

					single_query(temp_cmd);
				}
			}
			static public DataTable circle_query(SQLiteCommand cmd)
			{
				DataTable dataTable = new DataTable();
				SQLiteConnection conn = new SQLiteConnection(conn_str);
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
			static public void single_query(SQLiteCommand cmd)
			{
				try
				{
					SQLiteConnection conn = new SQLiteConnection(conn_str);
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
			static public string read_cihper_query(string query, string password)
			{
				cipher = new Cipher(password);
				SQLiteConnection conn_DB = new SQLiteConnection(conn_str);
				SQLiteCommand cmd = new SQLiteCommand(query, conn_DB);
				conn_DB.Open();

				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					(cipher.Salt, cipher.Hash_string, cipher.AESiv) = FetchSecurityKeys(reader);
				}
				reader.Close();
				cmd.Dispose();
				conn_DB.Close();
				conn_DB.Dispose();
				return cipher.DecryptAES(cipher.Hash_string, cipher.AES_key, cipher.AESiv);
			}

			public static (byte[], byte[], byte[]) FetchSecurityKeys(SQLiteDataReader reader)
			{
				byte[] salt = (byte[])reader["salt"];
				byte[] pass_hash = (byte[])reader["password_hash"];
				byte[] iv = (byte[])reader["aes_iv"];
				return (salt, pass_hash, iv);
			}
		}
	}
}
