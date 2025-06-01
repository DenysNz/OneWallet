using Finance.Data.Models;

namespace Finance.Web.ViewModels.UserDetails
{
    public class UserDetailsViewModel
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public bool IsLoggedBySocialNetwork { get; set; }
        public UserDetailsViewModel() { }

        public UserDetailsViewModel(UserDetail userDetails)
        {
            FirstName = userDetails.FirstName;
            LastName = userDetails.LastName;
            UserName = userDetails.DisplayName;
            Email = userDetails.Email;
            Description = userDetails.Description;
            IsLoggedBySocialNetwork = userDetails.IsLoggedBySocialNetwork;
        }

        public UserDetail ToEntity(int userId)
        {
            return new UserDetail
            {
                UserDetailId = userId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                DisplayName = this.UserName,
                Email = this.Email,
                Description = this.Description
            };
        }
    }
}
