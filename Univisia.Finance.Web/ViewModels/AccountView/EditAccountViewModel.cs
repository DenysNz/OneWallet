using Univisia.Finance.Data.Models;
using Univisia.Finance.Services;

namespace Univisia.Finance.Web.ViewModels.AccountView
{
    public class EditAccountViewModel
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
    }
}

