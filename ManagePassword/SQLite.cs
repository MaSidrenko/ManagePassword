using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ManagePassword
{
	static internal class SQLite
	{
		static string conn_str = "Data Source = PassData.db; Version = 3";
		static SQLiteCommand temp_cmd;
		static Cipher Cipher;
		static public DataTable SQLiteRefresh()
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
		static public void SQLiteInsert(string Service, string Password)
		{
			try
			{
				byte[] salt = null, AESkey = null, iv = null, cipher_pass = null;

				Cipher = new Cipher(out salt, out AESkey, out iv, ref cipher_pass, ref Password);

				temp_cmd = new SQLiteCommand("INSERT INTO PasswordCihper(open_string, password_hash, salt, aes_iv) VALUES(@open_string, @password_hash, @salt, @aes_iv)");
				Dictionary<string, object> parameters = QueriesDB.CreateParameters(Service, cipher_pass, salt, iv);
				SetParameters_forSQLite(temp_cmd, parameters);
				single_query(temp_cmd);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		static public DataTable SQLiteFind(string Service)
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
				return SQLiteRefresh();
			}
		}
		static public void SQLiteDelte(string delItem)
		{
			temp_cmd = new SQLiteCommand("DELETE FROM PasswordCihper WHERE id = @id");
			temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(delItem));
			single_query(temp_cmd);
		}
		static public void SQLiteChange(string id, string Service, string Password)
		{
			if (AdmMode.isAdm)
			{
				byte[] salt = null, AESkey = null, iv = null, cipher_pass = null;
				Cipher = new Cipher(out salt, out AESkey, out iv, ref cipher_pass, ref Password);

				temp_cmd = new SQLiteCommand("UPDATE PasswordCihper SET open_string = @open_string, password_hash = @password_hash, salt = @salt, aes_iv = @aes_iv WHERE id = @id");
				Dictionary<string, object> parametres = QueriesDB.CreateParameters(Service, cipher_pass, salt, iv);
				SetParameters_forSQLite(temp_cmd, parametres);
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
		static public void SetParameters_forSQLite(SQLiteCommand cmd, Dictionary<string, object> param)
		{
			foreach (KeyValuePair<string, object> prm in param)
			{
				cmd.Parameters.AddWithValue(prm.Key, prm.Value);
			}
		}
	}
}
