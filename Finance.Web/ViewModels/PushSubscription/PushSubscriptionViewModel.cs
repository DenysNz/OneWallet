using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.Json.Serialization;
using Finance.Data.Models;

namespace Finance.Web.ViewModels.PushSubscription
{
    public class PushSubscriptionViewModel
    {
        [JsonPropertyName("keys")]
        public Keys keys { get; set; }
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }
        [JsonPropertyName("expirationTime")]
        public  double? ExpirationTime { get; set; }
    }
}
