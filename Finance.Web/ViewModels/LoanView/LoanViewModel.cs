using System.Security.Policy;
using Finance.Data.Models;

namespace Finance.Web.ViewModels.LoanView
{
    public class LoanViewModel
    {
        public int LoanId { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public DateTime? Deadline { get; set; }
        public decimal Amount { get; set; }
        public string? Person { get; set; }
        public string? Note { get; set; }
        public int LoanStatusId { get; set; }
        public bool IsOwner { get; set; }

        public LoanViewModel(Loan entity, bool isOwner) 
        { 
            LoanId = entity.LoanId;
            CurrencyId = entity.CurrencyId;
            CurrencyName = entity.Currency.CurrencyName;
            Deadline= entity.Deadline;
            Amount = entity.Amount;
            Person = entity.Person;
            Note = entity.Note;
            LoanStatusId = entity.LoanStatusId;
            IsOwner = isOwner;
        }
    }
}