using System.Security.Policy;
using Finance.Data.Models;

namespace Finance.Web.ViewModels.LoanView
{
    public class LoanOperationViewModel
    {
        public int LoanId { get; set; }
        public int CurrencyId { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }  
}