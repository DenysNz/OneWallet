using Univisia.Finance.Data.Models;
using Univisia.Finance.Services.Models;

namespace Univisia.Finance.Services.Services.Interfaces
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
