using System.Security.Policy;
using Finance.Data.Models;

namespace Finance.Web.ViewModels.LoanView
{
    public class AddLoanViewModel
    {
        public int CurrencyId { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal Amount { get; set; }
        public string? Person { get; set; }
        public string? Note { get; set; }
        public string? ContactEmail { get; set; }
    }
}