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
using ManagePassword.Model.AppSide;
using ManagePassword.Model.DBSide;
using Npgsql;
using NpgsqlTypes;

namespace ManagePassword
{
	namespace Model
	{
		static internal class QueriesDB
		{
			//private static ApplicationContext context;
			private static DatabaseProvider currentProvider = DatabaseProvider.SQLite;
			//public static string BdMode = "SQLite";
			public static string providerStr = ConfigManager.GetSelecetedProvider();
			private static DatabaseProvider provider = (DatabaseProvider)Enum.Parse(typeof(DatabaseProvider),providerStr);
			private static ApplicationContext context = ApplicationContextFactory.Create(provider);

			static public List<PasswordRecrods> Insert(string tb_insert_open, string tb_insert_secret)
			{
				if (providerStr == "PostgreSQL")
				{
					return Model.PostgreSQL.Insert(tb_insert_open, tb_insert_secret);
				}
				else if (providerStr == "SQLite")
				{
					return Model.SQLite.Insert(tb_insert_open, tb_insert_secret);
				}
				return null;
			}
			static public List<PasswordRecrods> Refresh()
			{
				if (providerStr == "PostgreSQL")
				{
					return Model.PostgreSQL.Refresh();
				}
				else if (providerStr == "SQLite")
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

				if (providerStr == "PostgreSQL")
				{
					return Model.PostgreSQL.Find(tb_find_open);
				}
				else if (providerStr == "SQLite")
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
				if (providerStr == "PostgreSQL")
				{
					return Model.PostgreSQL.Delete(tb_del_id);
				}
				else if (providerStr == "SQLite")
				{
					return Model.SQLite.Delete(tb_del_id);
				}
				return null;
			}
			static public List<PasswordRecrods> Change(string tb_change_open, string tb_change_secret, string tb_change_id)
			{
				if (providerStr == "PostgreSQL")
				{
					return Model.PostgreSQL.Update(Convert.ToInt32(tb_change_id), tb_change_open, tb_change_secret);
				}
				else if (providerStr == "SQLite")
				{
					return Model.SQLite.Update(Convert.ToInt32(tb_change_id), tb_change_open, tb_change_secret);
				}
				return null;
			}
			static public void SwitchDB()
			{
				if(providerStr == "PostgreSQL")
				{
					ConfigManager.SetSelectedProvider("PostgreSQL");
					context.Dispose();
					currentProvider = DatabaseProvider.PostgreSQL;
					context = ApplicationContextFactory.Create(currentProvider);
				}
				else if(providerStr == "SQLite")
				{
					ConfigManager.SetSelectedProvider("SQLite");
					context.Dispose();
					currentProvider = DatabaseProvider.SQLite;
					context = ApplicationContextFactory.Create(currentProvider);
				}
			}
		}
	}
}