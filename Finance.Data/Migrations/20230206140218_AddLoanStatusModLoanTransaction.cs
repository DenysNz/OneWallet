using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddLoanStatusModLoanTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Loan_LoanId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.AddColumn<string>(
                name: "QuoteOnStatus",
                schema: "dbo",
                table: "Transaction",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactDetailId",
                schema: "dbo",
                table: "Loan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                schema: "dbo",
                table: "Loan",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoanStatusId",
                schema: "dbo",
                table: "Loan",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoanStatus",
                schema: "dbo",
                columns: table => new
                {
                    LoanStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatus", x => x.LoanStatusId);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LoanStatus",
                columns: new[] { "LoanStatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Requested" },
                    { 2, "Approved" },
                    { 3, "Rejected" },
                    { 4, "Private" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_ContactDetailId",
                schema: "dbo",
                table: "Loan",
                column: "ContactDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_LoanStatusId",
                schema: "dbo",
                table: "Loan",
                column: "LoanStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_ContactDetail",
                schema: "dbo",
                table: "Loan",
                column: "ContactDetailId",
                principalSchema: "dbo",
                principalTable: "UserDetail",
                principalColumn: "UserDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_LoanStatus",
                schema: "dbo",
                table: "Loan",
                column: "LoanStatusId",
                principalSchema: "dbo",
                principalTable: "LoanStatus",
                principalColumn: "LoanStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Loan",
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
                name: "FK_Loan_ContactDetail",
                schema: "dbo",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_LoanStatus",
                schema: "dbo",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Loan",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "LoanStatus",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Loan_ContactDetailId",
                schema: "dbo",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_Loan_LoanStatusId",
                schema: "dbo",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "QuoteOnStatus",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ContactDetailId",
                schema: "dbo",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                schema: "dbo",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "LoanStatusId",
                schema: "dbo",
                table: "Loan");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Loan_LoanId",
                schema: "dbo",
                table: "Transaction",
                column: "LoanId",
                principalSchema: "dbo",
                principalTable: "Loan",
                principalColumn: "LoanId");
        }
    }
}
