using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ManagePassword.Model.DBSide
{
	public class SQLiteDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationContextSQLite>
	{
		public ApplicationContextSQLite CreateDbContext(string[] args)
		{
			DbContextOptionsBuilder<ApplicationContextSQLite> optionsBuilder =
				new DbContextOptionsBuilder<ApplicationContextSQLite>();

			optionsBuilder.UseSqlite("Data Source = PassData.db;");

			return new ApplicationContextSQLite(optionsBuilder.Options);
		}
	}
}
