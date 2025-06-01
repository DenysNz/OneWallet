using Finance.Data.Models;
using Finance.Services;

namespace Finance.Web.ViewModels.AccountView
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }

        public AccountViewModel() { }

        public AccountViewModel(BankAccount account)
        {
            AccountId = account.BankAccountId;
            Name = account.BankAccountName;
            CurrencyId = account.CurrencyId;
            CurrencyName = account.Currency.CurrencyName;
            Amount = account.BankAccountAmount;
            IsDeleted = account.IsDeleted;
        }
    }
}
