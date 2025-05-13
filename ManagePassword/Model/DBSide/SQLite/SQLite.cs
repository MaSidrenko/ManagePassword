using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using ManagePassword.Model.DBSide;
using Microsoft.EntityFrameworkCore;

namespace ManagePassword
{
	namespace Model
	{
		static internal class SQLite
		{
			static private DbContextOptionsBuilder<ApplicationContextSQLite> optionsBuilder = new DbContextOptionsBuilder<ApplicationContextSQLite>();
			const string CONN_STR = "Data Source = passwordCiphers.db;";
			static public List<PasswordRecrods> Refresh()
			{
				List<PasswordRecrods> result = new List<PasswordRecrods>();
				if (AdmMode.isAdm)
				{
					using (ApplicationContextSQLite db = new ApplicationContextSQLite(optionsBuilder.UseSqlite(CONN_STR).Options))
					{
						List<PasswordCipher> passwordCiphers = db.passwordCiphers.ToList();
						result = read_password1(passwordCiphers);
						return result;
					}
				}
				else
				{
					using (ApplicationContextSQLite db = new ApplicationContextSQLite(optionsBuilder.UseSqlite(CONN_STR).Options))
					{
						List<PasswordRecrods> data = db.passwordCiphers
							.Select(_ => new PasswordRecrods
							{
								Id = _.Id,
								Service = _.Service
							})
							.ToList();
						result.AddRange(data);
					}
					return result;
				}
			}
			static public List<PasswordRecrods> Insert(string service, string password)
			{
				using (ApplicationContextSQLite db = new ApplicationContextSQLite(optionsBuilder.UseSqlite(CONN_STR).Options))
				{
					Cipher cipher = new Cipher(password);
					cipher.GenerateKeys();
					cipher.Encrypt();

					PasswordCipher CihperPassword = new PasswordCipher
					{
						Service = service,
						Password_hash = cipher.Hash_string,
						Salt = cipher.Salt,
						Aes_iv = cipher.AESiv
					};
					db.passwordCiphers.Add(CihperPassword);
					db.SaveChanges();
					List<PasswordRecrods> pass = Refresh();
					return pass;
				}
			}
			static public List<PasswordRecrods> Find(string service)
			{
				List<PasswordRecrods> result = new List<PasswordRecrods>();
				using (ApplicationContextSQLite db = new ApplicationContextSQLite(optionsBuilder.UseSqlite(CONN_STR).Options))
				{
					if (AdmMode.isAdm)
					{
						List<PasswordCipher> passwordCipher = db.passwordCiphers.Where(_ => _.Service.StartsWith(service)).ToList();
						result = read_password1(passwordCipher);
						return result;
					}
					else if (!AdmMode.isAdm)
					{
						List<PasswordRecrods> passwordCihper = db.passwordCiphers
							.Where(_ => _.Service.StartsWith(service))
							.Select(_ => new PasswordRecrods { Service = _.Service })
							.ToList();
						result.AddRange(passwordCihper);
						return result;
					}
					else
					{
						return Refresh();
					}
				}
			}
			static public List<PasswordRecrods> Delete(int delItem)
			{
				using (ApplicationContextSQLite db = new ApplicationContextSQLite(optionsBuilder.UseSqlite(CONN_STR).Options))
				{
					List<PasswordRecrods> passwords;
					PasswordCipher password = db.passwordCiphers.Find(delItem);
					if (password == null)
						return Refresh();
					db.passwordCiphers.Remove(password);
					db.SaveChanges();
					passwords = Refresh();
					return passwords;
				}
			}
			static public List<PasswordRecrods> Update(int id, string service, string password)
			{
				if (AdmMode.isAdm)
				{
					Cipher cipher = new Cipher(password);
					cipher.GenerateKeys();
					cipher.Encrypt();
					using (ApplicationContextSQLite db = new ApplicationContextSQLite(optionsBuilder.UseSqlite(CONN_STR).Options))
					{
						PasswordCipher CiphPass = db.passwordCiphers.Find(id);
						if (CiphPass == null)
						{
							CiphPass.Service = service;
							CiphPass.Password_hash = cipher.Hash_string;
							CiphPass.Salt = cipher.Salt;
							CiphPass.Aes_iv = cipher.AESiv;

							db.SaveChanges();
						}
						List<PasswordRecrods> passwords = Refresh();
						return passwords;
					}
				}
				else
				{
					return Refresh();
				}
			}
			static public void single_query(SQLiteCommand cmd, string Service = "", string Password = "")
			{
				try
				{
					SQLiteConnection conn = new SQLiteConnection("Data Source = admin.db;");
					conn.Open();

					cmd.Connection = conn;
					cmd.CommandType = CommandType.Text;
					cmd.ExecuteNonQuery();

					cmd.Dispose();
					conn.Close();
					conn.Dispose();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			static public string read_adm_password(string query, string password)
			{
				Cipher cipher = new Cipher(password);
				SQLiteConnection conn_DB = new SQLiteConnection("Data Source = admin.db;");
				SQLiteCommand cmd = new SQLiteCommand(query, conn_DB);
				conn_DB.Open();

				SQLiteDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					cipher.Hash_string = (byte[])reader["password_hash"];
					cipher.Salt = (byte[])reader["salt"];
					cipher.AESiv = (byte[])reader["aes_iv"];
				}
				cipher.AES_key = cipher.DeriveKey(password, cipher.Salt);
				reader.Close();
				cmd.Dispose();
				conn_DB.Close();
				conn_DB.Dispose();
				return cipher.Decrypt(cipher.Hash_string, cipher.AES_key, cipher.AESiv);
			}
			static public void create_adm_password(Cipher cipher, string admin)
			{
				SQLiteCommand if_cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS Admins(id INTEGER PRIMARY KEY CHECK(id = 1), admin_name TEXT, password_hash BLOB NOT NULL, salt BLOB, aes_iv BLOB NOT NULL)");
				Model.SQLite.single_query(if_cmd);

				SQLiteCommand cmd = new SQLiteCommand($"INSERT INTO Admins(id, admin_name, password_hash, salt, aes_iv) VALUES(@id, @username, @password_hash, @salt, @aes_iv)");
				cmd.Parameters.AddWithValue("@id", 1);
				cmd.Parameters.AddWithValue("@username", admin);
				cmd.Parameters.AddWithValue("@salt", cipher.Salt);
				cmd.Parameters.AddWithValue("@password_hash", cipher.Hash_string);
				cmd.Parameters.AddWithValue("@aes_iv", cipher.AESiv);

				Model.SQLite.single_query(cmd);
			}
			static public int HaveAdmPass()
			{
				int count = 0;
				string query = "SELECT COUNT(id) FROM Admins";
				SQLiteConnection conn = new SQLiteConnection(CONN_STR);
				SQLiteCommand cmd = new SQLiteCommand(query, conn);
				try
				{
					conn.Open();
					count = Convert.ToInt32(cmd.ExecuteScalar());
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
				finally
				{
					cmd.Dispose();
					conn.Close();
				}
				return count;
			}
			static private List<PasswordRecrods> read_password1(List<PasswordCipher> passwordCiphers)
			{
				List<PasswordRecrods> result = new List<PasswordRecrods>();
				Cipher decrypt = new Cipher(AdmMode.UnsaveGetAdmPassword());
				foreach (PasswordCipher record in passwordCiphers)
				{
					decrypt.Hash_string = record.Password_hash;
					decrypt.Salt = record.Salt;
					decrypt.AESiv = record.Aes_iv;

					decrypt.AES_key = decrypt.DeriveKey(decrypt.Input_string, decrypt.Salt);
					string password = decrypt.Decrypt(decrypt.Hash_string, decrypt.AES_key, decrypt.AESiv);

					result.Add(new PasswordRecrods
					{
						Id = record.Id,
						Service = record.Service,
						Password = password,
					});
				}
				return result;
			}
			//Старнно написанный метод
			//Используется для получения только для получения дешифрованного солбца пароля
			static public DataTable read_passwords(DataTable table)
			{
				string decrypted = "";
				Model.Cipher cihper = new Model.Cipher(AdmMode.UnsaveGetAdmPassword());
				foreach (DataRow row in table.Rows)
				{
					cihper.Hash_string = (byte[])row["password_hash"];
					cihper.Salt = (byte[])row["salt"];
					cihper.AESiv = (byte[])row["aes_iv"];


					if (cihper.Hash_string != null && cihper.Salt != null && cihper.AESiv != null)
					{
						cihper.AES_key = cihper.DeriveKey(cihper.Input_string, cihper.Salt);
						decrypted = cihper.Decrypt(cihper.Hash_string, cihper.AES_key, cihper.AESiv);
						row["password"] = decrypted;
					}
				}
				return table;
			}
		}
	}
}