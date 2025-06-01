using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;

namespace Finance.Data.Repositories
{
    public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
    {
        protected readonly new  FinanceDbContext _dbContext;
        public BankAccountRepository( FinanceDbContext dbContext) : base( dbContext ) 
        {
            _dbContext = dbContext;
        }

        public async Task AddAccountAsync(int userId,  string userName, int currencyId, string name, decimal amount)
        {
            var account = new BankAccount()
            {
                CreatedBy = userName,
                UserDetailId = userId,
                CurrencyId = currencyId,
                BankAccountName = name,
                CreatedDate = DateTime.UtcNow,
                BankAccountAmount = amount
            };

            var notes = "Bank account created with beginning balance of " + amount;

            account.Transactions.Add(new Transaction
            {
                CreatedBy = userName,
                TransactionAmount = amount,
                Notes = notes,
                CreatedDate = DateTime.UtcNow,
            });

            await _dbContext.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<BankAccount?> GetAccountByIdAsync(int accountId)
        {
            return await _dbContext
                        .FindAsync<BankAccount>(accountId);
        }

        public async Task<List<BankAccount>> GetAllAccountsAsync(int userId, bool includeDeleted)
        {
            return await _dbContext
                    .BankAccounts
                    .Include(p => p.Currency)
                    .Where(p => p.UserDetailId == userId)
                    .Where(p => !p.IsDeleted || includeDeleted)
                    .ToListAsync();
        }

        public async Task ChangeBalanceAsync(int accountId, string userName, decimal amount, string notes)
        {
            var account = await _dbContext.FindAsync<BankAccount>(accountId);

            account.BankAccountAmount += amount;
            account.UpdatedDate = DateTime.UtcNow;
            account.UpdatedBy = userName;

            var transaction = new Transaction
            {
                BankAccountId = accountId,
                CreatedBy = userName,
                TransactionAmount = amount,
                Notes = notes,
                CreatedDate = DateTime.UtcNow,
            };
            await _dbContext.Transactions.AddAsync(transaction);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(int accountId, string userName, string name, int currencyId)
        {
            var account = await _dbContext.FindAsync<BankAccount>(accountId);

            account.UpdatedBy = userName;
            account.UpdatedDate = DateTime.UtcNow;
            account.BankAccountName = name;
            account.CurrencyId = currencyId;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(int accountId, string userName)
        {
            var deleteAccount = await _dbContext.FindAsync<BankAccount>(accountId);
            deleteAccount.IsDeleted = true;
            deleteAccount.UpdatedBy = userName;
            deleteAccount.UpdatedDate = DateTime.UtcNow;

            var transactions = await _dbContext.Transactions.Where(x => x.BankAccountId == accountId).ToListAsync();
            foreach (var transaction in transactions)
            {
                transaction.IsDeleted = true;
                transaction.UpdatedDate = DateTime.UtcNow;
                transaction.UpdatedBy = userName;
            }

            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<BankAccount> GetAccounts(int userId)
        {
            return _dbContext
                .BankAccounts
                .Include(x => x.Currency)
                .Where(x => x.UserDetailId == userId && !x.IsDeleted);
        }
    }
}
