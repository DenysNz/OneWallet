using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.Services
{
    public class BankAccountService : IBankAccountService 
    {
        private IBankAccountRepository _repositoryBA;
        private ICurrencyService _currencyService;

        public BankAccountService(IBankAccountRepository repositoryBA, ICurrencyService currencyService)
        {
            _repositoryBA = repositoryBA;
            _currencyService = currencyService;
        }

        public async Task AddAccountAsync(int userId, string userName, int currencyId, string name, decimal amount)
        {
            await _repositoryBA.AddAccountAsync(userId, userName, currencyId, name, amount);
        }

        public async Task<BankAccount> GetAccountAsync(int accountId)
        {
            return await _repositoryBA.GetAccountByIdAsync(accountId);
        }

        public async Task<List<BankAccount>> GetAllAccountsAsync(int userId, bool includeDeleted)
        {
            return await _repositoryBA.GetAllAccountsAsync(userId, includeDeleted);
        }

        public async Task DeleteAccountAsync(int accountId, string userName)
        {
            await _repositoryBA.DeleteAccountAsync(accountId, userName);
        }

        public async Task ChangeBalanceAsync(int accountId, string userName, decimal amount, string notes)
        {
            await _repositoryBA.ChangeBalanceAsync(accountId, userName, amount, notes);
        }

        public async Task<List<Balance>> GetBalanceAsync(int userId)
        {
            var accounts = _repositoryBA.GetAccounts(userId);

            return await accounts
                .GroupBy(x => x.CurrencyId)
                .Select(x => new Balance(x.Key, x.Sum(x => x.BankAccountAmount)))
                .ToListAsync();
        }

        public async Task<List<LookupItem>> AccountsLookup(int userId)
        {
            var accounts = _repositoryBA.GetAccounts(userId);

            return await accounts
                .Select(x => new LookupItem(x.BankAccountId, x.BankAccountName))
                .ToListAsync();
        }

        public async Task UpdateAccountAsync(int accountId, string userName, string name, int currencyId)
        {
            await _repositoryBA.UpdateAccountAsync(accountId, userName, name, currencyId);
        }

        public async Task<List<WalletModel>> GetWalletAsync(int userId)
        {
            var accounts = _repositoryBA.GetAccounts(userId);

            var wallets = await accounts
                .GroupBy(x => new LookupItem( x.CurrencyId, x.Currency.CurrencyName ))
                .Select(x => new WalletModel(x.Key, x.Select(x => new AccountModel(x)).ToList()))
                .ToListAsync();

            wallets.ForEach(w => w.Total = w.Accounts.Sum(s => s.Amount));

            return wallets;
        }

        public async Task<decimal> CalculateTotal(List<WalletModel> wallets)
        {
            decimal result = 0;

            var rates = await _currencyService.GetRate(Constants.USD);

            foreach (var wallet in wallets)
            {
                result += wallet.Total / rates[wallet.Currency.Title.ToUpper()];
            }
            
            return result;
        }
    }
}