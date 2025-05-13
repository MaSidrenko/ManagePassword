using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagePassword.Model.AppSide;
using Microsoft.EntityFrameworkCore;

namespace ManagePassword.Model
{
	public class ApplicationContextPostgre : DbContext
	{
		public DbSet<PasswordCipher> passwordCiphers { get; set; } = null;
		public DbSet<Admin> Admins { get; set; } = null;
		public ApplicationContextPostgre(DbContextOptions<ApplicationContextPostgre> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<PasswordCipher>().ToTable("passwordCiphers");
			//modelBuilder.Entity<Admin>().HasIndex(_ => _.Id).IsUnique();
		}
	}
}
