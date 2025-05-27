using Univisia.Finance.Services.Models;

namespace Univisia.Finance.Services.Services.Interfaces
{
    public interface ISocialService
    {
        string GetGoogleSettings();
        Task<LoginResponse> GoogleLoginAsync(string credential);
        Task<LoginResponse> LoginOrRegisterUserBySocialNetworks(SocialNetworkUserDetails socialNetworkViewModel);
    }
}
