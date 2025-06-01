using Finance.Data.Models;
using Finance.Services.Models;

namespace Finance.Services.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> UserLoginAsync(LoginForm loginForm);

        Task<RegistrationResponse> UserRegisterAsync(RegistrationForm registrationForm);

        Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword);

        Task<bool> SendSecurityCodeAsync(string email);

        Task<string> CheckSecurityCodeAsync(string email, string code);

        Task<bool> ResetPasswordAsync(string email, string newPassword);

        Task InitAsync();
    }
}
