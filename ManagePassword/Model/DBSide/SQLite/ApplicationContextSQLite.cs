using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ManagePassword.Model.DBSide
{
	public class ApplicationContextSQLite : DbContext
	{
		public DbSet<PasswordCipher> passwordCiphers { get; set; }
		public ApplicationContextSQLite(DbContextOptions<ApplicationContextSQLite> options) : base(options) { }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<PasswordCipher>().ToTable("passwordCiphers");
		}
	}
}
