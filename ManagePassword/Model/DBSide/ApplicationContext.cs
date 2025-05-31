using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagePassword.Model.AppSide;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ManagePassword.Model
{
	
	public class ApplicationContext : DbContext
	{
		public DbSet<PasswordCipher> passwordCiphers { get; set; } = null;
		public DbSet<Admin> Admins { get; set; } = null;
		public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) 
		{
			//Database.EnsureCreated();
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if(!optionsBuilder.IsConfigured)
			{
				IConfiguration config = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", false)
					.Build();

				string selectedProvider = config["SelectedProvider"];
				string connectionString = config.GetConnectionString(selectedProvider);

				if (selectedProvider == "PostgreSQL")
				{
					optionsBuilder.UseNpgsql(connectionString);
				}
				else if(selectedProvider == "SQLite")
				{
					optionsBuilder.UseSqlite(connectionString);
				}
			}
			
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<PasswordCipher>().ToTable("passwordCiphers");
			modelBuilder.Entity<Admin>().ToTable("Admins");

			modelBuilder.Entity<Admin>()
				.HasIndex(_ => _.Name)
				.IsUnique();
			modelBuilder.Entity<Admin>()
				.Property(_ => _.Name)
				.HasDefaultValue("Admin");

		}
	}
}
