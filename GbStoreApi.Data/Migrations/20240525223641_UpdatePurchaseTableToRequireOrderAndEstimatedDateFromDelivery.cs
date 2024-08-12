using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GbStoreApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseTableToRequireOrderAndEstimatedDateFromDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("Purchase", "OrderDate", null, "OrderDate", DateTime.Now);
            migrationBuilder.UpdateData("Purchase", "EstimatedDeliveryDate", null, "EstimatedDeliveryDate", DateTime.Now);
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Purchase",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedDeliveryDate",
                table: "Purchase",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "BirthdayDate",
                value: new DateTime(2024, 5, 25, 18, 36, 41, 594, DateTimeKind.Local).AddTicks(5738));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("Purchase", "OrderDate", DateTime.Now, "OrderDate", null);
            migrationBuilder.UpdateData("Purchase", "EstimatedDeliveryDate", DateTime.Now, "EstimatedDeliveryDate", null);
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Purchase",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedDeliveryDate",
                table: "Purchase",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "BirthdayDate",
                value: new DateTime(2024, 5, 25, 17, 56, 32, 757, DateTimeKind.Local).AddTicks(960));
        }
    }
}
