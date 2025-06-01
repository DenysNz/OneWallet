using Finance.Data.Models;
using Finance.Services.Models;

namespace Finance.Services.Services.Interfaces
{
    public interface IPushSubscriptionService

    {
       Task AddSubscriptionAsync(int userId, string endpoint, double? expirationTime, string encriptionKey, string secret);

        Task SendPushNotification(PushSubscription subscrb);

        Task PushAll(int contactId);
    }
}
