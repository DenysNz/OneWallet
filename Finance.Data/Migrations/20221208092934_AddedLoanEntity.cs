using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddedLoanEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountName",
                schema: "dbo",
                table: "BankAccount",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Loan",
                schema: "dbo",
                columns: table => new
                {
                    LoanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Person = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.LoanId);
                    table.ForeignKey(
                        name: "FK_Loan_Currency",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currency",
                        principalColumn: "CurrencyId");
                    table.ForeignKey(
                        name: "FK_Loan_UserDetail",
                        column: x => x.UserDetailId,
                        principalSchema: "dbo",
                        principalTable: "UserDetail",
                        principalColumn: "UserDetailId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_CurrencyId",
                schema: "dbo",
                table: "Loan",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_UserDetailId",
                schema: "dbo",
                table: "Loan",
                column: "UserDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loan",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "BankAccountName",
                schema: "dbo",
                table: "BankAccount");
        }
    }
}
