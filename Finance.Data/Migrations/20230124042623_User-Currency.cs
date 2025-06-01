using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class UserCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                schema: "dbo",
                table: "Currency",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CurrencyUserDetail",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    UserDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyUserDetail", x => new { x.CurrencyId, x.UserDetailId });
                });

            migrationBuilder.CreateTable(
                name: "UserCurrency",
                schema: "dbo",
                columns: table => new
                {
                    UserDetailId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCurrency", x => new { x.UserDetailId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_UserCurrency_Currency",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currency",
                        principalColumn: "CurrencyId");
                    table.ForeignKey(
                        name: "FK_UserCurrency_UserDetail",
                        column: x => x.UserDetailId,
                        principalSchema: "dbo",
                        principalTable: "UserDetail",
                        principalColumn: "UserDetailId");
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 1,
                column: "IsPopular",
                value: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 2,
                column: "IsPopular",
                value: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 3,
                column: "IsPopular",
                value: true);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Currency",
                columns: new[] { "CurrencyId", "CurrencyName", "IsPopular" },
                values: new object[,]
                {
                    { 4, "JPY", false },
                    { 5, "GBP", true },
                    { 6, "CNY", false },
                    { 7, "AUD", false },
                    { 8, "CAD", true },
                    { 9, "CHF", false },
                    { 10, "HKD", false },
                    { 11, "SGD", false },
                    { 12, "SEK", false },
                    { 13, "KRW", false },
                    { 14, "NOK", false },
                    { 15, "NZD", false },
                    { 16, "INR", false },
                    { 17, "MXN", false },
                    { 18, "TWD", false },
                    { 19, "ZAR", false },
                    { 20, "BRL", false },
                    { 21, "DKK", false },
                    { 22, "PLN", false },
                    { 23, "THB", false },
                    { 24, "ILS", false },
                    { 25, "IDR", false },
                    { 26, "CZK", false },
                    { 27, "AED", false },
                    { 28, "TRY", false },
                    { 29, "HUF", false },
                    { 30, "CLP", false },
                    { 31, "SAR", false },
                    { 32, "PHP", false },
                    { 33, "MYR", false },
                    { 34, "COP", false },
                    { 35, "RUB", false },
                    { 36, "RON", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrency_CurrencyId",
                schema: "dbo",
                table: "UserCurrency",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyUserDetail");

            migrationBuilder.DropTable(
                name: "UserCurrency",
                schema: "dbo");

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 36);

            migrationBuilder.DropColumn(
                name: "IsPopular",
                schema: "dbo",
                table: "Currency");
        }
    }
}
