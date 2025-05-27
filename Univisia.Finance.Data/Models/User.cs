using Microsoft.AspNetCore.Identity;

namespace Univisia.Finance.Data.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
