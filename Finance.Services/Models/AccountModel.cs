using Finance.Data.Models;

namespace Finance.Services.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public decimal Amount  { get; set; }

        public AccountModel(BankAccount bankAccount) 
        {
            AccountId = bankAccount.BankAccountId;
            AccountName = bankAccount.BankAccountName;
            Amount = bankAccount.BankAccountAmount;
        }
    }
}
