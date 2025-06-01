using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddedUAHtoCurrencyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Currency",
                columns: new[] { "CurrencyId", "CurrencyName" },
                values: new object[] { 3, "UAH" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Currency",
                keyColumn: "CurrencyId",
                keyValue: 3);
        }
    }
}
