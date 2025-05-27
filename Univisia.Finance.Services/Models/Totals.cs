using Univisia.Finance.Data.Models;

namespace Univisia.Finance.Services.Models
{
    public class Totals
    {
        public string CurrencyName { get; set; }
        public decimal Amount { get; set; }

        public Totals(string currencyName, decimal amount)
        {
            CurrencyName = currencyName;
            Amount = amount;
        }
    }
}
