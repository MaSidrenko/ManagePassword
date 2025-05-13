using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagePassword.Model.DBSide
{
	public class PostgreDesingTimeFactory : IDesignTimeDbContextFactory<ApplicationContextPostgre>
	{
		public ApplicationContextPostgre CreateDbContext(string[] args)
		{
			DbContextOptionsBuilder<ApplicationContextPostgre> optionsBuilder =
				new DbContextOptionsBuilder<ApplicationContextPostgre>();

			optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=291305;Database=Passwords");

			return new ApplicationContextPostgre(optionsBuilder.Options);
		}
	}
}
