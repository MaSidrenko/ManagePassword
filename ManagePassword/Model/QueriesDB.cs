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
			public static string BdMode = "Postgre";
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
			static public DataTable Read(DataTable table)
			{
				if(BdMode == "Postgre")
				{
					return PostgreSQL.read_passwords(table);
				}
				else if(BdMode == "SQLite")
				{
					return SQLite.read_passwords(table);
				}
				else
				{
					return null;
				}

			}
		}
	}
}