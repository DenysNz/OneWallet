using System.Security.Policy;
using Univisia.Finance.Data.Models;

namespace Univisia.Finance.Web.ViewModels.LoanView
{
    public class ChangeLoanStatus
    {
        public int LoanId { get; set; }
        public string? Quotes { get; set; }
    }
}