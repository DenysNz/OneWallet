using Microsoft.AspNetCore.Identity;
using Finance.Data.Migrations;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;
using Finance.Services.JWtFeature;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.Services.Services
{
    public class UserDetailsService : IUserDetailsService
    {
        private readonly IGenericRepository<UserDetail> _userDetailsRepo;
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;

        public UserDetailsService(IGenericRepository<UserDetail> userDetailsRepo,
            UserManager<User> userManager,
            JwtHandler jwtHandler)
        {
            _userDetailsRepo = userDetailsRepo;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        public async Task CreateUserDetailAsync(UserDetail user)
        {
            await _userDetailsRepo.Create(user);
        }

        public async Task<int> GetUserDetailIdByEmailAsync(string email) 
        {
            var existingUser = _userDetailsRepo
                .GetAll()
                .FirstOrDefault(u => u.Email == email);

            if (existingUser == null) 
            {
                return 0;
            }

            return existingUser.UserDetailId;
        }

        public async Task<UserDetail?> GetUserDetailByEmailAsync(string email)
        {
            return _userDetailsRepo
                .GetAll()
                .FirstOrDefault(u => u.Email == email);
        }

        public async Task<string> UpdateUserDetailsAsync(UserDetail userDetail) 
        {
            var existingUser = await _userDetailsRepo.Get(userDetail.UserDetailId);
            var existingUserIdentity = await _userManager.FindByEmailAsync(existingUser.Email);

            if (existingUser != null && existingUserIdentity != null && existingUser.Email == existingUserIdentity.Email) 
            {
                existingUserIdentity.FirstName = userDetail.FirstName;
                existingUserIdentity.LastName = userDetail.LastName;
                existingUserIdentity.UserName = userDetail.DisplayName;

                if(!existingUser.IsLoggedBySocialNetwork)
                existingUserIdentity.Email = userDetail.Email;
                existingUserIdentity.UserName = userDetail.DisplayName;

                var resultUpdate = await _userManager.UpdateAsync(existingUserIdentity);

                if (resultUpdate.Succeeded)
                {
                    existingUser.FirstName = userDetail.FirstName;
                    existingUser.LastName = userDetail.LastName;
                    existingUser.Email = userDetail.Email;
                    existingUser.DisplayName = userDetail.DisplayName;
                    existingUser.Description = userDetail.Description;
                    existingUser.UpdatedDate = DateTime.Now;

                    if (!existingUser.IsLoggedBySocialNetwork)
                        existingUser.Email = userDetail.Email;

                    await _userDetailsRepo.Update(existingUser);

                    return await _jwtHandler.GetJwtTokenAsync(existingUserIdentity);
                }
                return string.Empty;
            }
            return string.Empty;
        }

        public async Task<SupportUser> RequestSupportAsync(int userID)
        {
            var currentUser = await _userDetailsRepo.Get(userID);

            return new SupportUser(currentUser);
        }

        public async Task<bool> GetWecomePageIsViisted(int userID)
        {
            var currentUser = await _userDetailsRepo.Get(userID);

            return currentUser.WelcomePageIsVisited;
        }
        public async Task UpdateWecomePageIsViisted(int userID)
        {
            var currentUser = await _userDetailsRepo.Get(userID);
            currentUser.WelcomePageIsVisited = true;

            _userDetailsRepo.Update(currentUser);
        }
    }
}
