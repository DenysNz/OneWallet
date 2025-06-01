using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using Finance.Data.Models;

namespace Finance.Web.ViewModels.AccountView
{
    public class TransactionViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }

        public TransactionViewModel(Transaction transaction)
        {
            Name = transaction.BankAccount == null ? FormatLoanName(transaction.Loan) : transaction.BankAccount.BankAccountName;
            Date = transaction.CreatedDate;
            CurrencyId = transaction.BankAccount == null ? transaction.Loan.CurrencyId : transaction.BankAccount.CurrencyId;
            CurrencyName = transaction.BankAccount == null ? transaction.Loan.Currency.CurrencyName : transaction.BankAccount.Currency.CurrencyName;
            Amount = transaction.TransactionAmount;
            Notes = transaction.Notes;
        }

        private string FormatLoanName(Loan loan)
        {
            return $"{loan.Person} ({loan.Currency.CurrencyName}{loan.Amount})";
        }
    }
}
