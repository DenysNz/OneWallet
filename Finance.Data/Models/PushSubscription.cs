using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Data.Models
{
    public class PushSubscription
    {
        [Key]
        public int PushSubscriptionId { get; set; }
        [Required]
        public string Endpoint { get; set; }
        [StringLength(255)]
        public double? ExpirationTime { get; set; }
        [Required]
        public string EncriptionKey { get; set; }
        [Required]
        public string Secret { get; set; }
        [Key]
        public int UserDeteilId { get; set; }

        [ForeignKey("UserDeteilId")]
        [InverseProperty("PushSubscriptions")]
        public UserDetail UserDeteil { get; set; }
    }
}
