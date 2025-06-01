using Finance.Data.Models;
using Finance.Services.Models;

namespace Finance.Web.ViewModels.AccountView
{
    public class BalanceViewModel
    {
       public decimal Balance { get; set; }
       public List<WalletModel> Wallets { get; set; }
    }
}
