using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class RemovedCurrencyIdfromTransactionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Currency",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_CurrencyId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                schema: "dbo",
                table: "Transaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                schema: "dbo",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CurrencyId",
                schema: "dbo",
                table: "Transaction",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Currency",
                schema: "dbo",
                table: "Transaction",
                column: "CurrencyId",
                principalSchema: "dbo",
                principalTable: "Currency",
                principalColumn: "CurrencyId");
        }
    }
}
