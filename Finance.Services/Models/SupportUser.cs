using Finance.Data.Models;

namespace Finance.Services.Models
{

    public class SupportUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProblemDescription { get; set; } = string.Empty;

        public SupportUser() { }

        public SupportUser(UserDetail userDetails)
        {
            UserName = userDetails.DisplayName;
            Email = userDetails.Email;
        }
    }
}
