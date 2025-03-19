using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;

namespace ManagePassword
{
	namespace Model
	{
		static internal class PostgreSQL
		{
			const string CONN_STR = "Host=localhost;Username=postgres;Password=291305;Database=postgres";
			static public DataTable Refresh()
			{
				DataTable dataTable = new DataTable();
				NpgsqlCommand temp_cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS passwordcihper (id SERIAL PRIMARY KEY, open_string TEXT, password_hash BYTEA NOT NULL, salt BYTEA NOT NULL,aes_iv BYTEA NOT NULL);");
				single_query(temp_cmd);
				if (AdmMode.isAdm && AdmMode.AdmPassword != "")
				{
					temp_cmd = new NpgsqlCommand("SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper");
					dataTable = circle_query(temp_cmd);
					return dataTable;
				}
				else
				{
					temp_cmd = new NpgsqlCommand("SELECT id AS \"Number\", open_string AS \"Service\" FROM PasswordCihper");
					return circle_query(temp_cmd);
				}
			}
			static public void Insert(string Service, string Password)
			{
				NpgsqlCommand temp_cmd = new NpgsqlCommand("INSERT INTO PasswordCihper (open_string, password_hash, salt, aes_iv) VALUES(@open_string, @password_hash, @salt, @aes_iv)");
				single_query(temp_cmd, Service, Password);
			}
			static public DataTable Find(string Service)
			{
				DataTable dataTable = new DataTable();
				if (AdmMode.isAdm)
				{
					NpgsqlCommand temp_cmd = new NpgsqlCommand("SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper WHERE open_string LIKE @open");
					temp_cmd.Parameters.AddWithValue("@open", NpgsqlDbType.Text, Service + "%");
					dataTable = circle_query(temp_cmd);
					return dataTable;
				}
				else if (!AdmMode.isAdm)
				{
					NpgsqlCommand temp_cmd = new NpgsqlCommand("SELECT id AS \"Nubmer\", open_string AS \"Service\" FROM PasswordCihper WHERE open_string LIKE @open");
					temp_cmd.Parameters.AddWithValue("@open", NpgsqlDbType.Text, Service + "%");
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
				NpgsqlCommand temp_cmd = new NpgsqlCommand("DELETE FROM passwordcihper WHERE id = @id");
				temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(delItem));
				single_query(temp_cmd);
			}
			static public void Change(string id, string Service, string Password)
			{
				if (AdmMode.isAdm && AdmMode.AdmPassword != "")
				{
					NpgsqlCommand temp_cmd = new NpgsqlCommand("UPDATE PasswordCihper SET open_string = @open_string, password_hash = @password_hash, salt = @salt, aes_iv = @aes_iv WHERE id = @id");
					temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));

					single_query(temp_cmd, Service, Password);

				}
			}
			static public Dictionary<string, object> Create_and_set_parameters(NpgsqlCommand cmd, string open_string, byte[] cipher_password, byte[] salt, byte[] iv)
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
			static public DataTable circle_query(NpgsqlCommand cmd)
			{
				DataTable dataTable = new DataTable();
				try
				{
					NpgsqlConnection conn_DB = new NpgsqlConnection(CONN_STR);
					conn_DB.Open();
					cmd.Connection = conn_DB;
					cmd.CommandType = CommandType.Text;
					NpgsqlDataReader reader = cmd.ExecuteReader();
					if (reader.HasRows)
					{
						dataTable.Load(reader);
					}
					reader.Close();
					cmd.Dispose();
					conn_DB.Close();
					conn_DB.Dispose();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
				return dataTable;
			}
			static public void single_query(NpgsqlCommand cmd, string Service = "", string Password = "")
			{

				try
				{
					if (Service != "" && Password != "")
					{
						Cipher cipher = new Cipher(Password);
						cipher.GenerateKeys();
						cipher.Encrypt();
						Dictionary<string, object> parameters = Create_and_set_parameters(cmd, Service, cipher.Hash_string, cipher.Salt, cipher.AESiv);
					}

					NpgsqlConnection conn_DB = new NpgsqlConnection(CONN_STR);
					conn_DB.Open();

					cmd.Connection = conn_DB;
					cmd.ExecuteNonQuery();

					cmd.Dispose();
					conn_DB.Close();
					conn_DB.Dispose();
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
				}
			}

			static public string read_cihper_query(string query, string cihper_string)
			{
				Cipher cipher = new Cipher(cihper_string);
				NpgsqlConnection conn_DB = new NpgsqlConnection(CONN_STR);
				NpgsqlCommand cmd = new NpgsqlCommand(query, conn_DB);
				conn_DB.Open();

				NpgsqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					cipher.Salt = (byte[])reader["salt"];
					cipher.Hash_string = (byte[])reader["password_hash"];
					cipher.AESiv = (byte[])reader["aes_iv"];
				}
				cipher.AES_key = cipher.DeriveKey(cihper_string, cipher.Salt);
				reader.Close();
				cmd.Dispose();
				conn_DB.Close();
				conn_DB.Dispose();
				return cipher.Decrypt(cipher.Hash_string, cipher.AES_key, cipher.AESiv);

			}
		}
	}
}
