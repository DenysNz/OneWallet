using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Finance.Data.Repositories.Interfaces;
using Finance.Services.Services;
using Finance.Services.Services.Interfaces;
using WebPush;
using PushSubscription = WebPush.PushSubscription;

namespace Finance.Services
{
    public class PushSubscriptionService : IPushSubscriptionService
    {
        private IPushSubscriptionRepository _repositoryPS;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public PushSubscriptionService(IPushSubscriptionRepository repositoryPS, IConfiguration configuration, ILogger<PushSubscriptionService> logger)
        {
           _repositoryPS = repositoryPS;
           _configuration = configuration;
            _logger = logger;
        }

        public async Task AddSubscriptionAsync(int userId, string endpoint, double? expirationTime, string encriptionKey, string secret)
        {
            _logger.LogInformation($"Got PushSubscription  for user: {userId}");
            await _repositoryPS.AddSubscriptionAsync(userId, endpoint, expirationTime, encriptionKey, secret);
            _logger.LogInformation($"PushSubscription added to DB for user: {userId}");
        }

        public async Task SendPushNotification(Finance.Data.Models.PushSubscription pushSubscription)
        {
            _logger.LogInformation("Starting to create PushNotification");
            var pushEndpoint = pushSubscription.Endpoint;
            var p256dh = pushSubscription.EncriptionKey;
            var auth = pushSubscription.Secret;
            _logger.LogInformation($"Got pushEndpon, key and secret from DB: {pushSubscription.Endpoint}\n {pushSubscription.EncriptionKey}\n{pushSubscription.Secret}");

            var subject = @"mailto:" + "superadmin@test.mail";
            var publicKey = @_configuration.GetSection("FcmSettings:SenderId").Value;
            var privateKey = @_configuration.GetSection("FcmSettings:ServerKey").Value;
            _logger.LogInformation($"Added subject line & public/private keys: {subject}\n {privateKey}");

            var subscription = new WebPush.PushSubscription(pushEndpoint, p256dh, auth);
            _logger.LogInformation("Created PushSubscription object using endpoint, key and secret");

            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
            _logger.LogInformation("Created VapidDetailes object using subject, public/private pair of keys");


            var payload = new { message = @"there was a new loan created on you" };
            string jPayload = JsonConvert.SerializeObject(payload);
            _logger.LogInformation($"Created optional payload: {jPayload}");

            using (var webPushClient = new WebPushClient())
            {
                try
                {
                    await webPushClient.SendNotificationAsync(subscription, jPayload, vapidDetails);
                }
                catch (WebPushException exeption)
                {
                    if (exeption.Message.Contains("subscription has unsubscribed or expired"))
                    {
                        _logger.LogError(exeption, "Deleted not valid PushSubscription");
                    }
                    else
                    {
                        _logger.LogError(exeption, "Other WebPushException");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Send PushNotification Error");
                }
            }
        }
        
        public async Task PushAll(int contactId)
        {
            var subscriptions = await _repositoryPS.GetSubscription(contactId);
            
            Parallel.ForEach(subscriptions, async subscription => { await SendPushNotification(subscription); });
        }
    }
}