using System;
using System.Collections;
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
        NpgsqlCommand temp_cmd;
        public void Insert(string tb_insert_open, string tb_insert_secret)
        {
            temp_cmd = new NpgsqlCommand($"CALL insertPasswords('{tb_insert_open}','{tb_insert_secret}')");
            single_query(temp_cmd);
        }
        public DataTable Refresh()
        {
            return circle_query(AdmMode.isAdm ? "SELECT id, open_string AS \"Service\", secret_string AS \"Password\" FROM \"Passwords\"" : "SELECT id, open_string AS \"Service\" FROM \"Passwords\"");
        }
        public DataTable Find(string tb_find_open, string tb_find_secret)
        {
            if (AdmMode.isAdm)
            {
                return circle_query($"SELECT id, open_string AS \"Service\", secret_string AS \"Password\" FROM \"Passwords\" WHERE \"open_string\" = '{tb_find_open}' OR \"secret_string\" = '{tb_find_secret}'");
            }
            else if(!AdmMode.isAdm) 
            {
                if (tb_find_secret != "")
                {
                    MessageBox.Show("You are not in Admin mode and you don't have access in column 'Password'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (tb_find_open != "")
                {
                return circle_query($"SELECT id, open_string AS \"Service\" FROM \"Passwords\" WHERE \"open_string\" = '{tb_find_open}'");

                }
                return Refresh();
            }
            else
            {
                MessageBox.Show("Unkown Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Refresh();
            }
        }
        public void Del(string tb_del_id)
        {
            temp_cmd = new NpgsqlCommand($"CALL deletepasswords('{tb_del_id}')");
            single_query(temp_cmd);
        }
        public void Change(string tb_change_open, string tb_change_secret, string tb_change_id)
        {
            if (AdmMode.isAdm)
            {
                temp_cmd = new NpgsqlCommand($"CALL UpdatePasswords('{tb_change_open}', '{tb_change_secret}', '{tb_change_id}')");
                single_query(temp_cmd);

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
        public void single_query(NpgsqlCommand cmd)
        {
            try
            {
                NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
                conn_DB.Open();
                
                cmd.Connection = conn_DB;
                cmd.ExecuteNonQuery();

                conn_DB.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public (byte[], string, byte[]) read_cihper_query(string query)
        {
            byte[] salt = null;
            string pass_hash = null;
            byte[] iv = null;
            NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn_DB);
            conn_DB.Open();


            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
               (salt, pass_hash, iv) = AdmMode.GetAutAdm(reader);
            }
            reader.Close();
            cmd.Dispose();
            conn_DB.Close();
            conn_DB.Dispose();
            return (salt, pass_hash, iv);
        }
    }
}
