using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GbStoreApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserAddressRelationToCorrectIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingPurchases_UserAddresses_UserOwnerAddressId",
                table: "ShippingPurchases");

            migrationBuilder.DropIndex(
                name: "IX_ShippingPurchases_UserOwnerAddressId",
                table: "ShippingPurchases");

            migrationBuilder.DropColumn(
                name: "UserAddressId",
                table: "ShippingPurchases");

            migrationBuilder.AddColumn<Guid>(
                name: "UserAddressId",
                table: "ShippingPurchases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty
                );

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPurchases_UserAddressId",
                table: "ShippingPurchases",
                column: "UserAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingPurchases_UserAddresses_UserAddressId",
                table: "ShippingPurchases",
                column: "UserAddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingPurchases_UserAddresses_UserAddressId",
                table: "ShippingPurchases");

            migrationBuilder.DropIndex(
                name: "IX_ShippingPurchases_UserAddressId",
                table: "ShippingPurchases");

            migrationBuilder.AlterColumn<int>(
                name: "UserAddressId",
                table: "ShippingPurchases",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserOwnerAddressId",
                table: "ShippingPurchases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPurchases_UserOwnerAddressId",
                table: "ShippingPurchases",
                column: "UserOwnerAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingPurchases_UserAddresses_UserOwnerAddressId",
                table: "ShippingPurchases",
                column: "UserOwnerAddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
