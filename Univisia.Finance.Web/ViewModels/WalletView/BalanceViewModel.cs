using Univisia.Finance.Data.Models;
using Univisia.Finance.Services.Models;

namespace Univisia.Finance.Web.ViewModels.AccountView
{
    public class BalanceViewModel
    {
       public decimal Balance { get; set; }
       public List<WalletModel> Wallets { get; set; }
    }
}
