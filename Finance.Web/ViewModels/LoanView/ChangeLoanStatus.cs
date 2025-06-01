using System.Security.Policy;
using Finance.Data.Models;

namespace Finance.Web.ViewModels.LoanView
{
    public class ChangeLoanStatus
    {
        public int LoanId { get; set; }
        public string? Quotes { get; set; }
    }
}