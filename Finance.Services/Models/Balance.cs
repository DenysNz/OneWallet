namespace Finance.Services.Models
{
    public class Balance
    {
        public int CurrencyId { get; set; }
        public decimal BalanceAmount { get; set; }

        public Balance(int id, decimal amount)
        {
            CurrencyId = id;
            BalanceAmount = amount;
        }
    }
}
