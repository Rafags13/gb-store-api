using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GbStoreApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewAdminToServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthdayDate", "Cpf", "Email", "Name", "Password", "RefreshToken", "TokenCreated", "TokenExpires", "TypeOfUser" },
                values: new object[] { 1, new DateTime(2024, 5, 6, 7, 0, 14, 492, DateTimeKind.Local).AddTicks(4441), "00000000000", "admin@gmail.com", "Administrador", "$2a$11$t6XFpMSG74tuVVOCidtmQeXdqyteWbTBIqe29uC98goiLtzqiZdzC", "", null, null, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
