using Finance.Data.Models;
using Finance.Services;

namespace Finance.Web.ViewModels.AccountView
{
    public class EditAccountViewModel
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int CurrencyId { get; set; }
    }
}

