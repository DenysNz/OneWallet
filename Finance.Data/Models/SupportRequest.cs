using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.Data.Models
{
    [Table("SupportRequest")]
    public class SupportRequest
    {
        [Key]
        public int RequestId { get; set; }
        public int? UserDetailId { get; set; }
        [Required]
        [StringLength(20)]
        public string RequestName { get; set; }
        [Required]
        [StringLength(256)]
        public string RequestEmail { get; set; }
        [Required]
        [StringLength(4000)]
        public string RequestQuestion { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        [ForeignKey("UserDetailId")]
        [InverseProperty("SupportRequests")]
        public UserDetail UserDetail { get; set; }
    }
}
