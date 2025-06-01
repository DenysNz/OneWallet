using Microsoft.EntityFrameworkCore;
using Finance.Data.Enums;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;

namespace Finance.Data.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        protected readonly new FinanceDbContext _dbContext;
        public TransactionRepository(FinanceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Transaction>> GetAllTransactionsAsync(int userId, int startIndex, int endIndex, int? accountId, int? loanId)
        {
            var query = await GetTarnsactionsAsync(userId, accountId, loanId);

            return await query
                .OrderByDescending(x => x.CreatedDate)
                .Skip(startIndex - 1)
                .Take(endIndex - startIndex + 1)
                .ToListAsync();
        }

        public async Task<IQueryable<Transaction>> GetTarnsactionsAsync(int userId, int? accountId, int? loanId)
        {
            var query = _dbContext
                .Transactions
                .Include(x => x.BankAccount)
                .ThenInclude(y => y.Currency)
                .Include(x => x.Loan)
                .ThenInclude(y => y.Currency)
                .Where(x => ((x.BankAccount != null && x.BankAccount.UserDetailId == userId && (accountId != null || !x.BankAccount.IsDeleted))
                            || (x.Loan != null && (x.Loan.UserDetailId == userId || (x.Loan.ContactDetailId == userId && !(x.Loan.LoanStatusId == LoanStatusEnum.Rejected))) && !x.Loan.IsDeleted)));

            if (accountId != null)
            {
                query = query.Where(x => x.BankAccountId == accountId);
            }

            if (loanId != null)
            {
                query = query.Where(x => x.LoanId == loanId);
            }

            return query;
        }

        public async Task<int> TransactionsCountAsync(int userId, int? accountId, int? loanId)
        {
            var query = await GetTarnsactionsAsync(userId, accountId, loanId);

            return await query.CountAsync();
        }
    }
}
