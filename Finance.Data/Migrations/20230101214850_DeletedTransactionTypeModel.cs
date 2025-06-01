using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class DeletedTransactionTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionType",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "TransactionType",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TransactionTypeId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TransactionTypeId",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "dbo",
                table: "Transaction",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "dbo",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeId",
                schema: "dbo",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TransactionType",
                schema: "dbo",
                columns: table => new
                {
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.TransactionTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionTypeId",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionType",
                schema: "dbo",
                table: "Transaction",
                column: "TransactionTypeId",
                principalSchema: "dbo",
                principalTable: "TransactionType",
                principalColumn: "TransactionTypeId");
        }
    }
}
