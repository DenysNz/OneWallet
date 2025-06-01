using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "dbo",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "varchar(max)", unicode: false, maxLength: 10000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                schema: "dbo",
                columns: table => new
                {
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionName = table.Column<string>(type: "varchar(max)", unicode: false, maxLength: 10000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.TransactionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                schema: "dbo",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    BankAccountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccount_Currency",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currency",
                        principalColumn: "CurrencyId");
                    table.ForeignKey(
                        name: "FK_BankAccount_UserDetail",
                        column: x => x.UserDetailId,
                        principalSchema: "dbo",
                        principalTable: "UserDetail",
                        principalColumn: "UserDetailId");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "dbo",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_BankAccount",
                        column: x => x.BankAccountId,
                        principalSchema: "dbo",
                        principalTable: "BankAccount",
                        principalColumn: "BankAccountId");
                    table.ForeignKey(
                        name: "FK_Transactions_Currency",
                        column: x => x.CurrencyId,
                        principalSchema: "dbo",
                        principalTable: "Currency",
                        principalColumn: "CurrencyId");
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionType",
                        column: x => x.TransactionTypeId,
                        principalSchema: "dbo",
                        principalTable: "TransactionType",
                        principalColumn: "TransactionTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_CurrencyId",
                schema: "dbo",
                table: "BankAccount",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_UserDetailId",
                schema: "dbo",
                table: "BankAccount",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BankAccountId",
                schema: "dbo",
                table: "Transaction",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CurrencyId",
                schema: "dbo",
                table: "Transaction",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionTypeId",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BankAccount",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TransactionType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "dbo");
        }
    }
}
