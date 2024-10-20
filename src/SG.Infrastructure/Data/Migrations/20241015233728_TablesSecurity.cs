using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TablesSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Catalogue",
                type: "character varying(800)",
                maxLength: 800,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(800)",
                oldMaxLength: 800);

            migrationBuilder.CreateTable(
                name: "PasswordHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    OldPassword = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    DateChange = table.Column<DateOnly>(type: "date", nullable: false),
                    UserSave = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PasswordPolicy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MinimumDigits = table.Column<int>(type: "integer", nullable: false),
                    RequiredLowercase = table.Column<bool>(type: "boolean", nullable: false),
                    RequiredUppercase = table.Column<bool>(type: "boolean", nullable: false),
                    RequiredCharacters = table.Column<bool>(type: "boolean", nullable: false),
                    SpecialCharacters = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    TimeNoRepeat = table.Column<int>(type: "integer", nullable: false),
                    ChangeTimeType = table.Column<char>(type: "character(1)", maxLength: 1, nullable: false),
                    PasswordChangeTime = table.Column<int>(type: "integer", nullable: false),
                    TemporaryPasswordChangeTime = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordPolicy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodeRol = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Firstname = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Lastname = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Password = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordHistory");

            migrationBuilder.DropTable(
                name: "PasswordPolicy");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Catalogue",
                type: "character varying(800)",
                maxLength: 800,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(800)",
                oldMaxLength: 800,
                oldNullable: true);
        }
    }
}
