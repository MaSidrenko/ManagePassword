using ManagePassword.Model.AppSide;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagePassword.Model.DBSide
{
	public enum DatabaseProvider
	{
		PostgreSQL,
		SQLite
	}

	public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
	{
		public ApplicationContext CreateDbContext(string[] args)
		{
			IConfiguration config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false)
				.Build();

			string selectedProvider = config["SelectedProvider"];
			string connectionString = config.GetConnectionString(selectedProvider);


			DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
			if(selectedProvider == "PostgreSQL")
			{
				optionsBuilder.UseNpgsql(connectionString);
			}
			else if(selectedProvider == "SQLite")
			{
				optionsBuilder.UseSqlite(connectionString);
			}
			return new ApplicationContext(optionsBuilder.Options);
		}
		public static ApplicationContext Create(DatabaseProvider databaseProvider)
		{
			DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

			switch (databaseProvider)
			{
				case DatabaseProvider.PostgreSQL:
					string postgresConn = ConfigManager.GetConnectionString("PostgreSQL");
					optionsBuilder.UseNpgsql(postgresConn);
					break;
				case DatabaseProvider.SQLite:
					string sqliteConn = ConfigManager.GetConnectionString("SQLite");
					optionsBuilder.UseSqlite(sqliteConn);
					break;
			}

			return new ApplicationContext(optionsBuilder.Options);
		}
	}
}
