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
			static public List<PasswordRecrods> Insert(string tb_insert_open, string tb_insert_secret)
			{
				if (BdMode == "Postgre")
				{
					return Model.PostgreSQL.Insert(tb_insert_open, tb_insert_secret);
				}
				else if (BdMode == "SQLite")
				{
					return Model.SQLite.Insert(tb_insert_open, tb_insert_secret);
				}
				return null;
			}
			static public List<PasswordRecrods> Refresh()
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
			static public List<PasswordRecrods> Find(string tb_find_open)
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
			static public List<PasswordRecrods> Del(int tb_del_id)
			{
				if (BdMode == "Postgre")
				{
					return Model.PostgreSQL.Delete(tb_del_id);
				}
				else if (BdMode == "SQLite")
				{
					return Model.SQLite.Delete(tb_del_id);
				}
				return null;
			}
			static public List<PasswordRecrods> Change(string tb_change_open, string tb_change_secret, string tb_change_id)
			{
				if (BdMode == "Postgre")
				{
					return Model.PostgreSQL.Update(Convert.ToInt32(tb_change_id), tb_change_open, tb_change_secret);
				}
				else if (BdMode == "SQLite")
				{
					return Model.SQLite.Update(Convert.ToInt32(tb_change_id), tb_change_open, tb_change_secret);
				}
				return null;
			}
		}
	}
}