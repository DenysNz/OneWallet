using Univisia.Finance.Data.Models;

namespace Univisia.Finance.Services.Models
{
    public class WalletModel
    {
        public List<AccountModel> Accounts{ get; set; }
        public LookupItem Currency { get; set; }
        public decimal Total { get; set; }

        public WalletModel(LookupItem currency, List<AccountModel> accounts)
        {
            Currency = currency;
            Accounts = accounts;
        }
    }
}
