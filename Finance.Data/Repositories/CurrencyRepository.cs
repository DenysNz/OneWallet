using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Finance.Data.Repositories
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        protected readonly new FinanceDbContext _dbContext;
        public CurrencyRepository(FinanceDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetAllCurrencyNamesAsync()
        {
            return _dbContext.Currencies
                .Select(x => x.CurrencyName)
                .ToList();
        }

        public IQueryable<Currency> GetCurrencies(bool justPopular)
        {
            return _dbContext
                .Currencies
                .Where(x => x.IsPopular || !justPopular);
        }

        public IQueryable<Currency> GetUserCurrencies(int userId)
        {
            return _dbContext.Currencies
                .Where(x => x.UserDetails.Any(j => j.UserDetailId == userId));
        }

        public async Task SaveUserCurrencies(List<Currency> newCurrencies, int userId)
        {
            var user = _dbContext
                .UserDetails
                .Include(x => x.Currencies)
                .Single(x => x.UserDetailId == userId);

            var toAddIds = newCurrencies
                .Select(x => x.CurrencyId)
                .Except(user.Currencies.Select(x => x.CurrencyId))
                .ToList();

            var toRemoveIds = user.Currencies
                .Select(x => x.CurrencyId)
                .Except(newCurrencies.Select(x => x.CurrencyId))
                .ToList();
 
            foreach (var id in toRemoveIds)
            {
                user.Currencies.Remove(user.Currencies.First(x => x.CurrencyId == id));
            }

            var currencies = _dbContext
                .Currencies
                .Where(x => toAddIds.Contains(x.CurrencyId))
                .ToList();

            foreach (var сurrency in currencies)
            {
                user.Currencies.Add(сurrency);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
