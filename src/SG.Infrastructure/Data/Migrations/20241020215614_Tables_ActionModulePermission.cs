using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tables_ActionModulePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IdParentModule = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Module_ParentModule",
                        column: x => x.IdParentModule,
                        principalTable: "Module",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    IdModule = table.Column<int>(type: "integer", nullable: false),
                    IdAction = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Actions",
                        column: x => x.IdAction,
                        principalTable: "Action",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Permission_Modules",
                        column: x => x.IdModule,
                        principalTable: "Module",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Module_IdParentModule",
                table: "Module",
                column: "IdParentModule");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_IdAction",
                table: "Permissions",
                column: "IdAction");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_IdModule",
                table: "Permissions",
                column: "IdModule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Action");

            migrationBuilder.DropTable(
                name: "Module");
        }
    }
}
