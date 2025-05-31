using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagePassword.Migrations
{
    public partial class SQLite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "passwordCiphers",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "passwordCiphers",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "Password_hash",
                table: "passwordCiphers",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "Aes_iv",
                table: "passwordCiphers",
                nullable: true
                );

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "passwordCiphers",
                nullable: false
                )
                .Annotation("Sqlite:Autoincrement", true)
                ;
            migrationBuilder.AddColumn<byte[]>(
                name: "salt",
                table: "Admins",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "password_hash",
                table: "Admins",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "aes_iv",
                table: "Admins",
                nullable: true
                );

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Admins",
                nullable: true,
                defaultValue: "Admin"
                );

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Admins",
                nullable: false
                )
                .Annotation("Sqlite:Autoincrement", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Service",
                table: "passwordCiphers",
                type: "text",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "passwordCiphers",
                type: "bytea",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "Password_hash",
                table: "passwordCiphers",
                type: "bytea",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "Aes_iv",
                table: "passwordCiphers",
                type: "bytea",
                nullable: true
                );

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "passwordCiphers",
                type: "integer",
                nullable: false
                )
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<byte[]>(
                name: "salt",
                table: "Admins",
                type: "bytea",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "password_hash",
                table: "Admins",
                type: "bytea",
                nullable: true
                );

            migrationBuilder.AddColumn<byte[]>(
                name: "aes_iv",
                table: "Admins",
                type: "bytea",
                nullable: true
                );

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Admins",
                type: "text",
                nullable: true,
                defaultValue: "Admin"
                );

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Admins",
                type: "integer",
                nullable: false
                )
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
