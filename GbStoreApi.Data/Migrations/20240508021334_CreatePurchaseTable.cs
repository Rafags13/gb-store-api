using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GbStoreApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatePurchaseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryAddressId = table.Column<int>(type: "int", nullable: false),
                    DeliveryInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_Addresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "BirthdayDate",
                value: new DateTime(2024, 5, 7, 22, 13, 34, 329, DateTimeKind.Local).AddTicks(5791));

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_DeliveryAddressId",
                table: "Purchase",
                column: "DeliveryAddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchase");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "BirthdayDate",
                value: new DateTime(2024, 5, 6, 7, 17, 10, 42, DateTimeKind.Local).AddTicks(9758));
        }
    }
}
