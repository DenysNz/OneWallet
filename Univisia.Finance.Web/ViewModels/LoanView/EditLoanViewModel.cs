using System.Security.Policy;
using Univisia.Finance.Data.Models;

namespace Univisia.Finance.Web.ViewModels.LoanView
{
    public class EditLoanViewModel
    {
        public int LoanId { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal Amount { get; set; }
        public string? Person { get; set; }
        public string? Note { get; set; }
    }
}