using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
                    Model.PostgreSQL.PostgreInsert(tb_insert_open, tb_insert_secret);
                }
                else if (BdMode == "SQLite")
                {
                    Model.SQLite.SQLiteInsert(tb_insert_open, tb_insert_secret);
                }
            }
            static public DataTable Refresh()
            {
                if (BdMode == "Postgre")
                {
                    return Model.PostgreSQL.PostgreRefresh();
                }
                else if (BdMode == "SQLite")
                {
                    return Model.SQLite.SQLiteRefresh();
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
                    return Model.PostgreSQL.PostgreFind(tb_find_open);
                }
                else if (BdMode == "SQLite")
                {
                    return Model.SQLite.SQLiteFind(tb_find_open);
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
                    Model.PostgreSQL.PostgreDelete(tb_del_id);
                }
                else if (BdMode == "SQLite")
                {
                    Model.SQLite.SQLiteDelte(tb_del_id);
                }
            }
            static public void Change(string tb_change_open, string tb_change_secret, string tb_change_id)
            {
                if (BdMode == "Postgre")
                {
                    Model.PostgreSQL.PostgreChange(tb_change_id, tb_change_open, tb_change_secret);
                }
                else if (BdMode == "SQLite")
                {
                    Model.SQLite.SQLiteChange(tb_change_id, tb_change_open, tb_change_secret);
                }
            } 
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