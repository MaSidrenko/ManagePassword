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
    internal class SqlQueries
    {
        string conn_str = "Host=localhost;Username=postgres;Password=291305;Database=postgres";
        NpgsqlCommand temp_cmd;
        public void SetParameters(NpgsqlCommand cmd, Dictionary<string, object> param)
        {
            foreach (KeyValuePair<string, object> prm in param)
            {
                cmd.Parameters.AddWithValue(prm.Key, prm.Value);
            }
        }
        public Dictionary<string, object> CreateParameters(string open_string, byte[] cipher_password, byte[] salt, byte[] iv)
        {
            return new Dictionary<string, object>
            {
                { "@open_string", open_string },
                { "@password_hash", cipher_password },
                { "@salt", salt },
                { "@aes_iv", iv }
            };
        }
        public void RemoveColumns(DataTable table)
        {
            table.Columns.Remove("password_hash");
            table.Columns.Remove("salt");
            table.Columns.Remove("aes_iv");
        }
        public void Insert(string tb_insert_open, string tb_insert_secret)
        {
            byte[] salt = null, AESkey = null, iv = null;
            (salt, iv, AESkey) = Cipher.GenerateKeys();
            byte[] cipher_pass = Cipher.EncryptAES(tb_insert_secret, AESkey, iv);

            temp_cmd = new NpgsqlCommand("INSERT INTO PasswordCihper (open_string, password_hash, salt, aes_iv) VALUES(@open_string, @password_hash, @salt, @aes_iv)");
            Dictionary<string, object> parameters = CreateParameters(tb_insert_open, cipher_pass, salt, iv);
            SetParameters(temp_cmd, parameters);

            single_query(temp_cmd);
        }
        public DataTable Refresh()
        {
            DataTable dataTable = new DataTable();

            if (AdmMode.isAdm)
            {
                dataTable = circle_query("SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper");
                dataTable = DecryptPasswordDB(dataTable);
                if (dataTable.Rows.Count > 0)
                    RemoveColumns(dataTable);
                return dataTable;
            }
            else
            {
                return circle_query("SELECT id AS \"Number\", open_string AS \"Service\" FROM PasswordCihper");
            }
        }

        public DataTable Find(string tb_find_open)
        {
            DataTable dataTable = new DataTable();
            if (AdmMode.isAdm)
            {
                dataTable = circle_query($"SELECT id AS \"Number\", open_string AS \"Service\", password_hash, salt, aes_iv FROM PasswordCihper WHERE open_string LIKE '{tb_find_open + "%"}'");
                dataTable = DecryptPasswordDB(dataTable);
                if (dataTable.Rows.Count > 0)
                    RemoveColumns(dataTable);
                return dataTable;
            }
            else if (!AdmMode.isAdm)
            {
                dataTable = circle_query($"SELECT id AS \"Nubmer\", open_string AS \"Service\" FROM PasswordCihper WHERE open_string LIKE '{tb_find_open + "%"}'");
                return dataTable;
            }
            else
            {
                return Refresh();
            }
        }
        public void Del(string tb_del_id)
        {
            temp_cmd = new NpgsqlCommand($"DELETE FROM passwordcihper WHERE id = '{tb_del_id}'");
            single_query(temp_cmd);
        }
        public void Change(string tb_change_open, string tb_change_secret, string tb_change_id)
        {
            if (AdmMode.isAdm)
            {
                byte[] salt = null, AESkey = null, iv = null;
                (salt, iv, AESkey) = Cipher.GenerateKeys();
                byte[] cipher_pass = Cipher.EncryptAES(tb_change_secret, AESkey, iv);

                temp_cmd = new NpgsqlCommand($"UPDATE PasswordCihper SET open_string = @open_string, password_hash = @password_hash, salt = @salt, aes_iv = @aes_iv WHERE id = '{tb_change_id}'");
                Dictionary<string, object> parameters = CreateParameters(tb_change_open, cipher_pass, salt, iv);
                SetParameters(temp_cmd, parameters);

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
        public (byte[], byte[], byte[]) read_cihper_query(string query)
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
        public string GetKeyForAdmin(string query)
        {
            string aesKeyBase64 = null;
            NpgsqlConnection conn_DB = new NpgsqlConnection(conn_str);
            conn_DB.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn_DB);
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                aesKeyBase64 = result.ToString();
            }
            cmd.Dispose();
            conn_DB.Dispose();
            conn_DB.Close();
            return aesKeyBase64;
        }
        private DataTable DecryptPasswordDB(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                byte[] encryptedData = (byte[])row["password_hash"];
                byte[] salt = (byte[])row["salt"];
                byte[] iv = (byte[])row["aes_iv"];


                if (!table.Columns.Contains("Password"))
                {
                    table.Columns.Add("Password", typeof(string));
                }

                if (encryptedData != null && salt != null && iv != null)
                {
                    byte[] key = Cipher.DeriveKey(AdmMode.AdmPassword, salt);
                    string decrypted = Cipher.DecryptAES(encryptedData, key, iv);
                    row["Password"] = decrypted;
                }
            }
            return table;
        }
    }
}