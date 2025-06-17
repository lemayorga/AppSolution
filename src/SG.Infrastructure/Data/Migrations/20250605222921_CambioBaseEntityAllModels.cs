using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SG.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CambioBaseEntityAllModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Permissions",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Module",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Action",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Permissions",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Module",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Action",
                newName: "State");
        }
    }
}
