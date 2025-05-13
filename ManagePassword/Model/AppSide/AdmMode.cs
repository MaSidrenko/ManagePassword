using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ManagePassword.Model.AppSide;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace ManagePassword
{
	namespace Model
	{
		static internal class AdmMode
		{
			private static string AdmPassword = "";
			public static bool isAdm { get; private set; }
			public static bool SetedAdmPassword { get; private set; }

			//public static NpgsqlCommand cmd = null;

			public static string UnsaveGetAdmPassword()
			{
				return AdmPassword;
			}
			public static void RegistrAdm(string password, string admin = "Admin")
			{
				try
				{
					Model.Cipher cipher = new Model.Cipher(password);
					cipher.GenerateKeys(password);
					cipher.Encrypt();

					if (Model.QueriesDB.BdMode == "Postgre")
					{
						//DbContextOptionsBuilder<ApplicationContextPostgre> optionsBuilder = new DbContextOptionsBuilder<ApplicationContextPostgre>();
						Model.PostgreSQL.create_adm_password(cipher, admin);
					/*	using (ApplicationContextPostgre db = new ApplicationContextPostgre(optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=291305;Database=Passwords").Options))
						{
							Admin new_admin = new Admin
							{
								password_hash = cipher.Hash_string,
								salt = cipher.Salt,
								aes_iv = cipher.AESiv,
							};
							db.Admins.Add(new_admin);
							db.SaveChanges();
						}*/
					}
					else if (Model.QueriesDB.BdMode == "SQLite")
					{

						Model.SQLite.create_adm_password(cipher, admin);
					}
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
				}
			}
			public static bool AuthenticateAdm(string password)
			{
				try
				{
					AdmPassword = password;
					string decrypted_pass = "";
					if (Model.QueriesDB.BdMode == "Postgre")
					{
						decrypted_pass = Model.PostgreSQL.read_adm_password($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'", password);
						/*Cipher decrypt = new Cipher(password);
						decrypted_pass = Model.PostgreSQL.read_adm_password1(password);*/
					}
					else if (Model.QueriesDB.BdMode == "SQLite")
					{
						decrypted_pass = Model.SQLite.read_adm_password($"SELECT password_hash, salt, aes_iv FROM Admins WHERE admin_name = 'Admin'", password);
					}
					if (decrypted_pass == password)
					{
						isAdm = true;
						SetedAdmPassword = true;
					}

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
				if (isAdm)
				{
					string delteQuery = "DELETE FROM Admins WHERE admin_name = 'Admin'";
					if (Model.QueriesDB.BdMode == "Postgre")
					{
						NpgsqlCommand cmd = new NpgsqlCommand(delteQuery);

						Model.PostgreSQL.single_query(cmd);
						cmd.Dispose();
					}
					else if (Model.QueriesDB.BdMode == "SQLite")
					{
						SQLiteCommand cmd = new SQLiteCommand(delteQuery);

						Model.SQLite.single_query(cmd);
						cmd.Dispose();
					}
					ClearMasterPassword();
				}
			}
			public static void ClearMasterPassword()
			{
				SetedAdmPassword = false;
				isAdm = false;
			}

			public static bool HaveAdm()
			{
				int count = 0;
				if (Model.QueriesDB.BdMode == "Postgre")
				{
					count = Model.PostgreSQL.HaveAdmPass();
				}
				if (Model.QueriesDB.BdMode == "SQLite")
				{
					count = Model.SQLite.HaveAdmPass();
				}
				return count == 1 ? true : false;
			}
		}
	}
}
