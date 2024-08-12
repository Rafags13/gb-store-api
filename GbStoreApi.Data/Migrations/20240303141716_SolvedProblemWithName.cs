using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GbStoreApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class SolvedProblemWithName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Users",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "Nome");
        }
    }
}
