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
        public void Insert(string tb_insert_open, string tb_insert_secret)
        {
            single_query($"CALL insertPasswords('{tb_insert_open}','{tb_insert_secret}')");
        }
        public DataTable Refresh()
        {
            if (AdmMode.isAdm)
                return circle_query("SELECT id, open_string AS \"Service\", secret_string AS \"Password\" FROM \"Passwords\"");
            else
                return circle_query("SELECT id, open_string AS \"Service\" FROM \"Passwords\"");
        }
        public DataTable FindAdm(string tb_find_open, string tb_find_secret)
        {
            if (AdmMode.isAdm)
            {
                return circle_query($"SELECT id, open_string AS \"Service\", secret_string AS \"Password\" FROM \"Passwords\" WHERE \"open_string\" = '{tb_find_open}' OR \"secret_string\" = '{tb_find_secret}'");
            }
            else
            {
                if (tb_find_secret != "")
                {
                    MessageBox.Show("You are not in Admin mode and you don't have access in column 'Password'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return circle_query($"SELECT id, open_string AS \"Service\" FROM \"Passwords\" WHERE \"open_string\" = '{tb_find_open}'");

            }
        }
        public void Del(string tb_del_id)
        {
            single_query($"CALL deletepasswords('{tb_del_id}')");
        }
        public void Change(string tb_change_open, string tb_change_secret, string tb_change_id)
        {
            if (AdmMode.isAdm)
                single_query($"CALL UpdatePasswords('{tb_change_open}', '{tb_change_secret}', '{tb_change_id}')");
            else if(!AdmMode.isAdm)
                MessageBox.Show("You are not in Admin mode and you don't have access in column 'Password'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public DataTable Select(string tb_change_id)
        {
            DataTable dt = new DataTable();
            if (AdmMode.isAdm)
            {
                dt = circle_query($"SELECT id, open_string AS \"Service\", secret_string AS \"Password\" FROM \"Passwords\" WHERE \"id\" = '{tb_change_id}'");
                return dt;
            }
            else
            {
                MessageBox.Show("You are not in Admin mode and you don't have access in column 'Password'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return Refresh();
            }
        }
        public DataTable circle_query(string query)
        {
            DataTable dataTable = new DataTable();
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
                    dataTable.Load(reader);
                }
                cmd.Dispose();
                conn_DB.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dataTable;
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
