using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ManagePassword
{
    static internal class AdmMode
    {
        public static bool isAdm = true;
        public static void RegistrUser(string password, string admin = "admin")
        {
            try
            {
                byte[] salt = Cipher.GenerateSalt();
                byte[] iv = Cipher.GenerateIV();
                byte[] key = Cipher.DeriveKey(password, salt);
                string cihper_pass = Cipher.EncryptAES(password, key, iv);

                string query = $"INSERT INTO Admins (admin_name, password_hash, salt, aes_iv) VALUES(@username, @password_hash, @salt, @aes_iv)";

                SqlQueries sql = new SqlQueries();
                NpgsqlCommand cmd = new NpgsqlCommand(query);

                cmd.Parameters.AddWithValue("@username", NpgsqlDbType.Text, "Admin");
                cmd.Parameters.AddWithValue("@salt", NpgsqlDbType.Bytea, salt);
                cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Bytea, Convert.FromBase64String(cihper_pass));
                cmd.Parameters.AddWithValue("@aes_iv", NpgsqlDbType.Bytea, iv);
                sql.single_query(cmd);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
    }
}
