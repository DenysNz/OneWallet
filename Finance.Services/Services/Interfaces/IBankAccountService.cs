using Finance.Data.Models;
using Finance.Services.Models;

namespace Finance.Services.Services.Interfaces
{
    public interface IBankAccountService
    {
        Task AddAccountAsync(int userId, string userName, int currencyId, string name, decimal amount);

        Task<List<BankAccount>> GetAllAccountsAsync(int userId, bool includeDeleted);

        Task<BankAccount> GetAccountAsync(int accountId);

        Task DeleteAccountAsync(int accountId, string userName);

        Task ChangeBalanceAsync(int accountId, string userName, decimal amount, string notes);

        Task<List<Balance>> GetBalanceAsync(int userId);

        Task<List<LookupItem>> AccountsLookup(int userId);

        Task UpdateAccountAsync(int accountId, string userName, string name, int currencyId);

        Task<List<WalletModel>> GetWalletAsync(int userId);

        Task<decimal> CalculateTotal(List<WalletModel> wallets);
    }
}
