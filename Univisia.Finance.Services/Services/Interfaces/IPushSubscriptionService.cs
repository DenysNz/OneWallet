using Univisia.Finance.Data.Models;
using Univisia.Finance.Services.Models;

namespace Univisia.Finance.Services.Services.Interfaces
{
    public interface IPushSubscriptionService

    {
       Task AddSubscriptionAsync(int userId, string endpoint, double? expirationTime, string encriptionKey, string secret);

        Task SendPushNotification(PushSubscription subscrb);

        Task PushAll(int contactId);
    }
}
