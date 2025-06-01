using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddedTriggers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER TRIGGER trg_LoanInsert
            ON [dbo].[Loan]
            AFTER INSERT
            AS
            BEGIN
            SET NOCOUNT ON;
            DECLARE @loancount INT;


	            SELECT @loancount = Count(1) FROM  inserted JOIN Loan ON inserted.UserDetailId = Loan.UserDetailId GROUP BY inserted.UserDetailId

	            IF @loancount > 250
	            BEGIN
		            RAISERROR ('You can not create more then 250 Loans', 10, 1)
	            END
            END
            GO");

            migrationBuilder.Sql(@"CREATE OR ALTER TRIGGER trg_BankAccountInsert
            ON [dbo].[BankAccount]
            AFTER INSERT
            AS
            BEGIN
            SET NOCOUNT ON;
            DECLARE @accountcount INT;


	            SELECT @accountcount = Count(1) FROM  inserted JOIN BankAccount ON inserted.UserDetailId = BankAccount.UserDetailId GROUP BY inserted.UserDetailId

	            IF @accountcount > 250
	            BEGIN
		            RAISERROR ('You can not create more then 250 BankAccounts', 10, 1)
	            END
            END
            GO");

            migrationBuilder.Sql(@"CREATE OR ALTER TRIGGER trg_TransactionInsert
            ON [dbo].[Transaction]
            AFTER INSERT
            AS
            BEGIN
            SET NOCOUNT ON;
            DECLARE @transactioncount INT;
            DECLARE @bankaccountId INT

	            SELECT @bankaccountId = bankAccountId from inserted

	            IF (@bankaccountId IS NOT NULL)
	            BEGIN
		            SELECT @transactioncount = Count(1) FROM  inserted JOIN [dbo].[Transaction] ON inserted.BankAccountId = [dbo].[Transaction].BankAccountId GROUP BY inserted.BankAccountId
	            END
	            ELSE
	            BEGIN
		            SELECT @transactioncount = Count(1) FROM  inserted JOIN [dbo].[Transaction] ON inserted.LoanId = [dbo].[Transaction].LoanId GROUP BY inserted.LoanId
	            END
	            IF @transactioncount > 5000
	            BEGIN
		            RAISERROR ('You can not create more then 5000 Transactions', 10, 1)
	            END
            END
            GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            DROP TRIGGER trg_LoanInsert");
            migrationBuilder.Sql(@"
            DROP TRIGGER trg_BankAccountInsert");
            migrationBuilder.Sql(@"
            DROP TRIGGER trg_TransactionInsert");
        }
    }
}
