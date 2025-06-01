using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance.Data.Models
{
    [Table("UserDetail", Schema = "dbo")]
    public class UserDetail
    {
        public UserDetail()
        {
            BankAccounts = new HashSet<BankAccount>();
            LoanContactDetails = new HashSet<Loan>();
            LoanUserDetails = new HashSet<Loan>();
            Currencies = new HashSet<Currency>();
            PushSubscriptions = new HashSet<PushSubscription>();
        }

        [Key]
        public int UserDetailId { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string DisplayName { get; set; }
        [StringLength(10000)]
        [Unicode(false)]
        public string? AvatarUrl { get; set; }
        [Required]
        [StringLength(10000)]
        [Unicode(false)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Status { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsLoggedBySocialNetwork { get; set; }
        public bool WelcomePageIsVisited { get; set; } = false;

        [InverseProperty("UserDetail")]
        public ICollection<BankAccount> BankAccounts { get; set; }

        [InverseProperty("ContactDetail")]
        public virtual ICollection<Loan> LoanContactDetails { get; set; }

        [InverseProperty("UserDetail")]
        public virtual ICollection<Loan> LoanUserDetails { get; set; }

        [ForeignKey("UserDetailId")]
        [InverseProperty("UserDetails")]
        public ICollection<Currency> Currencies { get; set; }

        [InverseProperty("UserDetail")]
        public ICollection<SupportRequest> SupportRequests { get; set; }

        [InverseProperty("UserDeteil")]
        public ICollection<PushSubscription> PushSubscriptions { get; set; }
    }
}
