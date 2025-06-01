using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finance.Data.Models;

namespace Finance.Data.Repositories.Interfaces
{
    public interface IBankAccountRepository : IGenericRepository<BankAccount>
    {
        Task AddAccountAsync(int userId, string userName, int currencyId, string name, decimal amount);

        Task<BankAccount?> GetAccountByIdAsync(int accountId);

        Task<List<BankAccount>> GetAllAccountsAsync(int userId, bool includeDeleted);

        Task DeleteAccountAsync(int accountId, string userName);

        Task ChangeBalanceAsync(int accountId, string userName, decimal amount, string notes);

        IQueryable<BankAccount> GetAccounts(int userId);

        Task UpdateAccountAsync(int accountId, string userName, string name, int currencyID);
    }
}
