using Finance.Data.Models;
using Finance.Services.Models;

namespace Finance.Services.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<CurrencyModel>> CurrencyLookup(bool justPopular);

        Task<Dictionary<string, decimal>> GetRate(string toCurrency);

        Task<List<LookupItem>> GetUserCurrencies(int userId);

        Task SaveUserCurrencies(List<LookupItem> currencies, int userId);

        Task<Dictionary<string, decimal>> GetCurrencyRates(List<string> fromCurrency, string toCurrency);
    }
}
