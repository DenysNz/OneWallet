using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Finance.Data.Models;

namespace Finance.Data.Repositories.Interfaces
{
    public interface IPushSubscriptionRepository : IGenericRepository<PushSubscription>
    {
        Task AddSubscriptionAsync(int userId, string endpoint, double? expirationTime, string encriptionKey, string secret);

        Task DeleteSubscription(int subscriptionId, int userId);

        Task<List<PushSubscription>> GetSubscription(int contactId);
    }
}
