using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagePassword.Model.AppSide;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;

namespace ManagePassword
{
	namespace Model
	{
		static internal class PostgreSQL
		{
			static DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

			const string CONN_STR = "Host=localhost;Username=postgres;Password=291305;Database=Passwords";
			static public List<PasswordRecrods> Refresh()
			{

				List<PasswordRecrods> result = new List<PasswordRecrods>();
				if (AdmMode.isAdm)
				{

					using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
					{
						List<PasswordCipher> passwordsCihper = db
							.passwordCiphers
							.ToList();
						result = read_passwords(passwordsCihper);
						return result;
					}
				}
				else
				{
					using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
					{
						List<PasswordRecrods> data = db
						.passwordCiphers
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
				using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
				{
					Cipher cipher = new Cipher(password);
					cipher.GenerateKeys();
					cipher.Encrypt();

					PasswordCipher Ciphpassword = new PasswordCipher
					{
						Service = service,
						Password_hash = cipher.Hash_string,
						Salt = cipher.Salt,
						Aes_iv = cipher.AESiv
					};
					db.passwordCiphers.Add(Ciphpassword);
					db.SaveChanges();
					List<PasswordRecrods> pass = Refresh();
					return pass;
				}
			}
			static public List<PasswordRecrods> Find(string Service)
			{
				List<PasswordRecrods> result = new List<PasswordRecrods>();
				using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
				{

					if (AdmMode.isAdm)
					{
						List<PasswordCipher> passwordsCihper = db
							.passwordCiphers
							.Where(_ => _.Service.StartsWith(Service))
							.ToList();
						result = read_passwords(passwordsCihper);
						return result;
					}
					else if (!AdmMode.isAdm)
					{
						List<PasswordRecrods> passwordsCihper = db
							.passwordCiphers
							.Where(_ => _.Service.StartsWith(Service))
							.Select(_ => new PasswordRecrods { Service = _.Service })
							.ToList();
						result.AddRange(passwordsCihper);
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
				using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
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
					using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
					{
						PasswordCipher CihpPass = db
							.passwordCiphers
							.Find(id);
						if (CihpPass != null)
						{
							CihpPass.Service = service;
							CihpPass.Password_hash = cipher.Hash_string;
							CihpPass.Salt = cipher.Salt;
							CihpPass.Aes_iv = cipher.AESiv;

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
			static public void delete_adm_password()
			{
				DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
				using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
				{
					Admin admin = db.Admins.Find("Admin");
					db.Admins.Remove(admin);
					db.SaveChanges();
				}
			}
			static public void create_adm_password(Cipher cipher, string admin)
			{
				using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
				{
					Admin new_adm = new Admin
					{
						password_hash = cipher.Hash_string,
						salt = cipher.Salt,
						aes_iv = cipher.AESiv,
					};
					db.Admins.Add(new_adm);
					db.SaveChanges();
				}
			}
			static public string read_adm_passwords(string cihper_passowrd)
			{
				string password = "";
				Cipher decrypt = new Cipher(cihper_passowrd);
				using (ApplicationContext db = new ApplicationContext(optionsBuilder.UseNpgsql(CONN_STR).Options))
				{
					List<Admin> admins = db.Admins.ToList();
					foreach (Admin admin in admins)
					{
						decrypt.Salt = admin.salt;
						decrypt.Hash_string = admin.password_hash;
						decrypt.AESiv = admin.aes_iv;
					}
					decrypt.AES_key = decrypt.DeriveKey(cihper_passowrd, decrypt.Salt);
					password = decrypt.Decrypt(decrypt.Hash_string, decrypt.AES_key, decrypt.AESiv);
				}
				return password;
			}
			static public int HaveAdmPass()
			{
				int count = 0;
				string query = "SELECT COUNT(id) FROM Admins";
				NpgsqlConnection conn = new NpgsqlConnection("Host=localhost;Username=postgres;Password=291305;Database=postgres");
				NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
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
			static private List<PasswordRecrods> read_passwords(List<PasswordCipher> passwordsCihper)
			{
				List<PasswordRecrods> result = new List<PasswordRecrods>();
				Cipher decrypt = new Cipher(AdmMode.UnsaveGetAdmPassword());
				foreach (PasswordCipher record in passwordsCihper)
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
		}
	}
}
