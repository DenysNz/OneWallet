using Finance.Data.Models;

namespace Finance.Data.Repositories.Interfaces
{
    public interface ICurrencyRepository : IGenericRepository<Currency>
    {
        IQueryable<Currency> GetCurrencies(bool justPopular);

        IQueryable<Currency> GetUserCurrencies(int userId);

        Task SaveUserCurrencies(List<Currency> newCurrencies, int userId);

        Task<List<string>> GetAllCurrencyNamesAsync();
    }
}
