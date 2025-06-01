using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class TransactionLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BankAccountId",
                schema: "dbo",
                table: "Transaction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LoanId",
                schema: "dbo",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_LoanId",
                schema: "dbo",
                table: "Transaction",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Loan_LoanId",
                schema: "dbo",
                table: "Transaction",
                column: "LoanId",
                principalSchema: "dbo",
                principalTable: "Loan",
                principalColumn: "LoanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Loan_LoanId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_LoanId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "LoanId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "BankAccountId",
                schema: "dbo",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
