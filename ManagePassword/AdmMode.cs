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
        public static bool isAdm = false;
        public static void RegistrAdm(string password, string admin = "admin")
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
                cmd.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static bool AuthenticateAdm(string password)
        {
            SqlQueries temp_sql = new SqlQueries();
            (byte[] salt, string pass_hash, byte[] aesIV) = temp_sql.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'");
            if (salt == null || pass_hash == null || aesIV == null)
                return false;
            try
            {
                byte[] encryptedPasswordBytes = Convert.FromBase64String(pass_hash);
                byte[] aesKey = Cipher.DeriveKey(password, salt);
                string decrypted_pass = Cipher.DecryptAES(Convert.ToBase64String(encryptedPasswordBytes), aesKey, aesIV);

                return decrypted_pass == password;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        public static void DeleteAdm(string password)
        {
            SqlQueries temp_sql = new SqlQueries();
            (byte[] salt, string pass_hash, byte[] aesIV) = temp_sql.read_cihper_query($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'");
            if (salt == null || pass_hash == null || aesIV == null)
                MessageBox.Show("User not found!");
            byte[] aesKey = Cipher.DeriveKey(password, salt);
            string decrypted_pass = Cipher.DecryptAES(pass_hash, aesKey, aesIV);

            if(decrypted_pass == password)
            {
                string delteQuery = "DELETE FROM Admins WHERE admin_name = 'Admin' AND password_hash = @password_hash";

                NpgsqlCommand cmd = new NpgsqlCommand(delteQuery);
                cmd.Parameters.AddWithValue("@password_hash", NpgsqlDbType.Bytea, Convert.FromBase64String(pass_hash));

                temp_sql.single_query(cmd);
                cmd.Dispose();
            }
        }
        public static (byte[], string, byte[]) GetAutAdm(NpgsqlDataReader reader)
        {
            byte[] salt = (byte[])reader["salt"];
            string pass_hash = Convert.ToBase64String((byte[])reader["password_hash"]);
            byte[] iv = (byte[])reader["aes_iv"];
            return (salt, pass_hash, iv);
        }
    }
}
