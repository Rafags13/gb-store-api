using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GbStoreApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InvertResponsabilityToDerivedTypesOfPurchaseToTheirDerivedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Purchase_DeliveryAddressId",
                table: "Purchases"
                );
            migrationBuilder.DropIndex(
                name: "IX_Purchases_ShippingPurchaseId",
                table: "Purchases"
                );
            migrationBuilder.DropIndex(
                name: "IX_Purchases_StorePickupPurchaseId",
                table: "Purchases"
                );
            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "DeliveryInstructions",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "ShippingPurchaseId",
                table: "Purchases"
                );

            migrationBuilder.DropColumn(
                name: "StorePickupPurchaseId",
                table: "Purchases"
                );

            migrationBuilder.CreateTable(
                name: "ShippingPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAddressId = table.Column<int>(type: "int", nullable: false),
                    UserOwnerAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingPurchases_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingPurchases_UserAddresses_UserOwnerAddressId",
                        column: x => x.UserOwnerAddressId,
                        principalTable: "UserAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorePickupPurchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreAddressId = table.Column<int>(type: "int", nullable: false),
                    UserBuyerId = table.Column<int>(type: "int", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePickupPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorePickupPurchases_Addresses_StoreAddressId",
                        column: x => x.StoreAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorePickupPurchases_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorePickupPurchases_Users_UserBuyerId",
                        column: x => x.UserBuyerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPurchases_PurchaseId",
                table: "ShippingPurchases",
                column: "PurchaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShippingPurchases_UserOwnerAddressId",
                table: "ShippingPurchases",
                column: "UserOwnerAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePickupPurchases_PurchaseId",
                table: "StorePickupPurchases",
                column: "PurchaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StorePickupPurchases_StoreAddressId",
                table: "StorePickupPurchases",
                column: "StoreAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePickupPurchases_UserBuyerId",
                table: "StorePickupPurchases",
                column: "UserBuyerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingPurchases");

            migrationBuilder.DropTable(
                name: "StorePickupPurchases");

            migrationBuilder.CreateTable(
                name: "Purchase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeOfDelivery = table.Column<int>(type: "int", nullable: false),
                    TypeOfPayment = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.Id);
                });

            migrationBuilder.DropIndex(
                name: "IX_ShippingPurchases_PurchaseId",
                table: "ShippingPurchases");

            migrationBuilder.DropIndex(
                name: "IX_ShippingPurchases_UserOwnerAddressId",
                table: "ShippingPurchases");

            migrationBuilder.DropIndex(
                name: "IX_StorePickupPurchases_PurchaseId",
                table: "StorePickupPurchases");

            migrationBuilder.DropIndex(
                name: "IX_StorePickupPurchases_StoreAddressId",
                table: "StorePickupPurchases");

            migrationBuilder.DropIndex(
                name: "IX_StorePickupPurchases_UserBuyerId",
                table: "StorePickupPurchases");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Purchases_PurchaseId",
                table: "OrderItems");
        }
    }
}
