using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;

namespace Finance.Data.Repositories
{
    public class PushSubscriptionRepository : GenericRepository<PushSubscription>, IPushSubscriptionRepository
    {
        protected readonly new FinanceDbContext _dbContext;
        public PushSubscriptionRepository( FinanceDbContext dbContext) : base( dbContext )
        {
            _dbContext = dbContext;
        }
        public async Task AddSubscriptionAsync(int userId, string endpoint, double? expirationTime, string encriptionKey, string secret)
        {
            var pushsubscription = new PushSubscription()
            {
                UserDeteilId = userId,
                Endpoint = endpoint,
                ExpirationTime = expirationTime,
                EncriptionKey = encriptionKey,
                Secret = secret
            };

            await _dbContext.AddAsync<PushSubscription>(pushsubscription);

            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteSubscription(int subscriptionId, int userId)
        {
            var subscription = await _dbContext.FindAsync<PushSubscription>(subscriptionId, userId);
                
            _dbContext.Remove(subscription);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<PushSubscription>> GetSubscription(int contactId)
        {
            return await _dbContext.PushSubscriptions
                .Where(x => x.UserDeteilId == contactId)
                .ToListAsync();
        }
    }
}
