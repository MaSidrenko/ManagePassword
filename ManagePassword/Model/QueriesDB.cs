using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Npgsql;
using NpgsqlTypes;

namespace ManagePassword
{
	namespace Model
	{
		static internal class QueriesDB
		{
			public static string BdMode = "SQLite";
			static public void Insert(string tb_insert_open, string tb_insert_secret)
			{
				if (BdMode == "Postgre")
				{
					Model.PostgreSQL.Insert(tb_insert_open, tb_insert_secret);
				}
				else if (BdMode == "SQLite")
				{
					Model.SQLite.Insert(tb_insert_open, tb_insert_secret);
				}
			}
			static public DataTable Refresh()
			{
				if (BdMode == "Postgre")
				{
					return Model.PostgreSQL.Refresh();
				}
				else if (BdMode == "SQLite")
				{
					return Model.SQLite.Refresh();
				}
				else
				{
					return null;
				}
			}

			static public DataTable Find(string tb_find_open)
			{

				if (BdMode == "Postgre")
				{
					return Model.PostgreSQL.Find(tb_find_open);
				}
				else if (BdMode == "SQLite")
				{
					return Model.SQLite.Find(tb_find_open);
				}
				else
				{
					return null;
				}
			}
			static public void Del(string tb_del_id)
			{
				if (BdMode == "Postgre")
				{
					Model.PostgreSQL.Delete(tb_del_id);
				}
				else if (BdMode == "SQLite")
				{
					Model.SQLite.Delete(tb_del_id);
				}
			}
			static public void Change(string tb_change_open, string tb_change_secret, string tb_change_id)
			{
				if (BdMode == "Postgre")
				{
					Model.PostgreSQL.Change(tb_change_id, tb_change_open, tb_change_secret);
				}
				else if (BdMode == "SQLite")
				{
					Model.SQLite.Change(tb_change_id, tb_change_open, tb_change_secret);
				}
			}
			static public DataTable ReadData(DataTable table)
			{
				string decrypted = "";
				Model.Cipher cihper = new Model.Cipher(Model.AdmMode.AdmPassword);
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
			//Не помню зачем нужен был этот метод.Пока оставлю, мало ли
			//static public string GetKeyForAdmin(string query)
			//{
			//    string aesKeyBase64 = null;
			//    NpgsqlConnection conn_DB = new NpgsqlConnection("Host=localhost;Username=postgres;Password=291305;Database=postgres");
			//    conn_DB.Open();
			//    NpgsqlCommand cmd = new NpgsqlCommand(query, conn_DB);
			//    object result = cmd.ExecuteScalar();
			//    if (result != null)
			//    {
			//        aesKeyBase64 = result.ToString();
			//    }
			//    cmd.Dispose();
			//    conn_DB.Dispose();
			//    conn_DB.Close();
			//    return aesKeyBase64;
			//}
		}
	}
}