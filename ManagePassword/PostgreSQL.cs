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
    static internal class PostgreSQL
    {
        static string conn_str = "Host=localhost;Username=postgres;Password=291305;Database=postgres";
        static NpgsqlCommand temp_cmd;
        static Cipher Cipher = null;
        static public DataTable PostgreRefresh()
        {
            DataTable dataTable = new DataTable();
            temp_cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS passwordcihper (id SERIAL PRIMARY KEY, open_string TEXT, password_hash BYTEA NOT NULL, salt BYTEA NOT NULL,aes_iv BYTEA NOT NULL);");
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
        static public void PostgreInsert(string Service, string Password)
        {
            byte[] salt = null, AESkey = null, iv = null, cipher_pass = null;
            Cipher = new Cipher(out salt, out AESkey, out iv, ref cipher_pass, ref Password);

            temp_cmd = new NpgsqlCommand("INSERT INTO PasswordCihper (open_string, password_hash, salt, aes_iv) VALUES(@open_string, @password_hash, @salt, @aes_iv)");
            Dictionary<string, object> parameters = QueriesDB.CreateParameters(Service, cipher_pass, salt, iv);
            SetParameters_forNpg(temp_cmd, parameters);
            single_query(temp_cmd);
        }
        static public DataTable PostgreFind(string Service)
        {
            DataTable dataTable = new DataTable();
            if (AdmMode.isAdm && AdmMode.AdmPassword != "")
            {
                temp_cmd = new NpgsqlCommand("SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper WHERE open_string LIKE @open");
                temp_cmd.Parameters.AddWithValue("@open", NpgsqlDbType.Text, Service + "%");
                dataTable = circle_query(temp_cmd);
                return dataTable;
            }
            else if (!AdmMode.isAdm && AdmMode.AdmPassword == "")
            {
                temp_cmd = new NpgsqlCommand("SELECT id AS \"Nubmer\", open_string AS \"Service\" FROM PasswordCihper WHERE open_string LIKE @open");
                temp_cmd.Parameters.AddWithValue("@open", NpgsqlDbType.Text, Service + "%");
                dataTable = circle_query(temp_cmd);
                return dataTable;
            }
            else
            {
                return PostgreRefresh();
            }
        }
        static public void PostgreDelete(string delItem)
        {
            temp_cmd = new NpgsqlCommand("DELETE FROM passwordcihper WHERE id = @id");
            temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(delItem));
            single_query(temp_cmd);
        }
        static public void PostgreChange(string id, string Service, string Password)
        {
            if (AdmMode.isAdm && AdmMode.AdmPassword != "")
            {
                //TODO
                byte[] salt = null, AESkey = null, iv = null, cipher_pass = null;
				Cipher = new Cipher(out salt, out AESkey, out iv, ref cipher_pass, ref Password);

				temp_cmd = new NpgsqlCommand("UPDATE PasswordCihper SET open_string = @open_string, password_hash = @password_hash, salt = @salt, aes_iv = @aes_iv WHERE id = @id");
                Dictionary<string, object> parameters = QueriesDB.CreateParameters(Service, cipher_pass, salt, iv);
                temp_cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                SetParameters_forNpg(temp_cmd, parameters);

                single_query(temp_cmd);

            }
        }
        static public void SetParameters_forNpg(NpgsqlCommand cmd, Dictionary<string, object> param)
        {
            foreach (KeyValuePair<string, object> prm in param)
            {
                cmd.Parameters.AddWithValue(prm.Key, prm.Value);
            }
        }
        static public DataTable circle_query(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
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
        static public void single_query(NpgsqlCommand cmd)
        {
            try
            {
                NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
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
        static public (byte[], byte[], byte[]) read_cihper_query(string query)
        {
            byte[] salt = null;
            byte[] pass_hash = null;
            byte[] iv = null;
            NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn_DB);
            conn_DB.Open();


            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                (salt, pass_hash, iv) = FetchSecurityKeys(reader);
            }
            reader.Close();
            cmd.Dispose();
            conn_DB.Close();
            conn_DB.Dispose();
            return (salt, pass_hash, iv);
        }
        public static (byte[], byte[], byte[]) FetchSecurityKeys(NpgsqlDataReader reader)
        {
            byte[] salt = (byte[])reader["salt"];
            byte[] pass_hash = (byte[])reader["password_hash"];
            byte[] iv = (byte[])reader["aes_iv"];
            return (salt, pass_hash, iv);
        }
    }
}
