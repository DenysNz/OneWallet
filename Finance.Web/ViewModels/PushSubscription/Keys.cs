using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text.Json.Serialization;
using Finance.Data.Models;

namespace Finance.Web.ViewModels.PushSubscription
{
    public class Keys
    {
        [JsonPropertyName("auth")]
        public string Secret { get; set; }
        [JsonPropertyName("p256dh")]
        public string EncriptionKey { get; set; }
    }
}
