using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ManagePassword
{
    internal class SqlQueries
    {
        string conn_str = "Host=localhost;Username=postgres;Password=291305;Database=postgres";
        ManagePassword MP_dialog;
        public SqlQueries(ManagePassword parent)
        {
            MP_dialog = parent;
        }
        public void circle_query(string query)
        {
            try
            {
                NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
                NpgsqlCommand cmd = new NpgsqlCommand();
                conn_DB.Open();
                cmd.Connection = conn_DB;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                NpgsqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    MP_dialog.Dgv_DB.DataSource = dataTable;
                }
                cmd.Dispose();
                conn_DB.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void single_query(string query)
        {
            try
            {
                NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
                NpgsqlCommand cmd = new NpgsqlCommand();
                conn_DB.Open();
                cmd.Connection = conn_DB;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                conn_DB.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
