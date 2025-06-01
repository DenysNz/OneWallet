using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Finance.Data.Enums;
using Finance.Data.Models;
using Finance.Services.JWtFeature;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.Services.Services
{
    public class SocialService : ISocialService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserDetailsService _userDetailsService;
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;

        public SocialService(IConfiguration configuration,
            IUserDetailsService userDetailsService,
            UserManager<User> userManager,
            JwtHandler jwtHandler)
        {
            _configuration = configuration;
            _userDetailsService = userDetailsService;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }
        
        public string GetGoogleSettings() 
        {
            return  _configuration["Google:ClientId"];
        }

        public async Task<LoginResponse> GoogleLoginAsync(string credential) 
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(credential);

            var tokenIsValid = GoogleTokenIsValid(decodedToken);

            if (tokenIsValid) 
            {
                var googleUser = GetGoogleUserDetails(decodedToken);

                return await LoginOrRegisterUserBySocialNetworks(googleUser);
            }
            return new LoginResponse();
        }

        public async Task<LoginResponse> LoginOrRegisterUserBySocialNetworks(SocialNetworkUserDetails socialNetworkViewModel) 
        {
            var existingUser = await _userManager.FindByEmailAsync(socialNetworkViewModel.Email);
            string candidToken = "";

            if (existingUser != null)
            {
                candidToken = await _jwtHandler.GetJwtTokenAsync(existingUser);
                return new LoginResponse { IsAuthSuccessful = true, Token = candidToken };
            }
            else
            {
                var newIdentityUser = new User
                {
                    Email = socialNetworkViewModel.Email,
                    FirstName = socialNetworkViewModel.FirstName,
                    LastName = socialNetworkViewModel.LastName,
                    UserName = socialNetworkViewModel.UserName,
                };

                var generatedPassword = GeneratePassword();
                var result = await _userManager.CreateAsync(newIdentityUser, generatedPassword);

                if (!result.Succeeded)
                {
                    var error = result.Errors.Select(e => e.Description).First();

                    return new LoginResponse { ErrorMessage = error };
                }
                else
                {
                    var identityUser = await _userManager.FindByNameAsync(socialNetworkViewModel.UserName);

                    if (identityUser != null)
                    {
                        var newUserDetails = new UserDetail
                        {
                            Email = socialNetworkViewModel.Email,
                            FirstName = socialNetworkViewModel.FirstName,
                            LastName = socialNetworkViewModel.LastName,
                            DisplayName = socialNetworkViewModel.UserName,
                            UserId = identityUser.Id,
                            Description = String.Empty,
                            Status = String.Empty,
                            CreatedDate = DateTime.UtcNow,
                            UpdatedDate = DateTime.UtcNow,
                            AvatarUrl = socialNetworkViewModel.AvatarUrl,
                            IsLoggedBySocialNetwork = true
                        };

                        await _userDetailsService.CreateUserDetailAsync(newUserDetails);
                        await _userManager.AddToRolesAsync(identityUser, new string[] { RolesEnum.Customer });

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                        await _userManager.ConfirmEmailAsync(identityUser, code);

                        candidToken = await _jwtHandler.GetJwtTokenAsync(identityUser);
                        return new LoginResponse { IsAuthSuccessful = true, Token = candidToken };
                    }
                }
            }
            return new LoginResponse();
        }

        private bool GoogleTokenIsValid(JwtSecurityToken token)
        {
            bool emailIsVerified = (bool)token.Payload["email_verified"];
            var iss = token.Payload["iss"].ToString();
            bool isSignedByGoogle = iss == "accounts.google.com" || iss == "https://accounts.google.com";
            bool clientIdIsCorrect = token.Payload["aud"].ToString() == _configuration["Google:ClientId"];
            var expTime = DateTimeOffset.FromUnixTimeSeconds((long)token.Payload["exp"]).UtcDateTime;
            bool tokenIsNotOverdue = DateTime.Compare(DateTime.UtcNow, expTime) < 0;

            return emailIsVerified && isSignedByGoogle && clientIdIsCorrect && tokenIsNotOverdue;
        }

        private SocialNetworkUserDetails GetGoogleUserDetails(JwtSecurityToken token)
        {
            return new SocialNetworkUserDetails
            {
                UserName = token.Payload["name"].ToString(),
                Email = token.Payload["email"].ToString(),
                FirstName = token.Payload["given_name"].ToString(),
                LastName = token.Payload["family_name"].ToString(),
                AvatarUrl = token.Payload["picture"].ToString(),
            };
        }

        private string GeneratePassword() 
        {
            var prefix = "CandidAutoGenerated_";
            var year = DateTime.UtcNow.Year;
            var month = DateTime.UtcNow.Month;
            var day = DateTime.UtcNow.Day;
            var hour = DateTime.UtcNow.Hour;
            var seconds = DateTime.UtcNow.Second;
            var milliseconds = DateTime.UtcNow.Millisecond;

            return prefix + year + month + day + hour + seconds + milliseconds;
        }
    }
}
