using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ManagePassword.Migrations.ApplicationContextSQLiteMigrations
{
    public partial class CreatePasswordCiphersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
			   name: "passwordCiphers",
			   columns: table => new
			   {
				   Id = table.Column<int>(nullable: false)
					   .Annotation("Sqlite:Autoincrement", true),
				   Cipher = table.Column<string>(nullable: false),
				   CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
			   },
			   constraints: table =>
			   {
				   table.PrimaryKey("PK_passwordCiphers", x => x.Id);
			   });
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropTable(
			  name: "passwordCiphers");
		}
    }
}
