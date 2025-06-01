using Finance.Data.Models;
using Finance.Services.Models;

namespace Finance.Services.Services.Interfaces
{
    public interface IUserDetailsService
    {
        Task CreateUserDetailAsync(UserDetail user);

        Task<int> GetUserDetailIdByEmailAsync(string email);

        Task<UserDetail?> GetUserDetailByEmailAsync(string email);

        Task<string> UpdateUserDetailsAsync(UserDetail userDetail);

        Task<SupportUser> RequestSupportAsync(int userID);

        Task<bool> GetWecomePageIsViisted(int userID);

        Task UpdateWecomePageIsViisted(int userID);
    }
}
