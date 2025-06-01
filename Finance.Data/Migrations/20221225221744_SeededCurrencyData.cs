using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class SeededCurrencyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Currency",
                columns: new[] { "CurrencyId", "CurrencyName" },
                values: new object[] { 1, "USD" });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Currency",
                columns: new[] { "CurrencyId", "CurrencyName" },
                values: new object[] { 2, "EUR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 2);
        }
    }
}
