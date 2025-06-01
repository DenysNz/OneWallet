using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Finance.Data.Enums;
using Finance.Data.Models;
using Finance.Services.JWtFeature;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserDetailsService _userDetailsService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtHandler _jwtHandler;
        private readonly ISendGridService _sendGridService;
        private readonly IMemoryCache _memoryCache;

        public AuthenticationService(IConfiguration configuration,
            IUserDetailsService userDetailsService,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            JwtHandler jwtHandler,
            ISendGridService sendGridService,
            IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _userDetailsService = userDetailsService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtHandler = jwtHandler;
            _sendGridService = sendGridService;
            _memoryCache = memoryCache;
        }

        public async Task<LoginResponse> UserLoginAsync(LoginForm loginForm)
        {
            var user = await _userManager.FindByNameAsync(loginForm.UserName);

            if (user == null)
                return new LoginResponse { ErrorMessage = "Invalid Username" };

            if (!await _userManager.CheckPasswordAsync(user, loginForm.Password))
                return new LoginResponse { ErrorMessage = "Invalid Password" };

            var canSignIn = await _signInManager.CanSignInAsync(user);

            if (canSignIn)
            {
                var token = await _jwtHandler.GetJwtTokenAsync(user);

                return new LoginResponse { IsAuthSuccessful = true, Token = token };
            }

            return new LoginResponse { ErrorMessage = "Email is not verified" };
        }

        public async Task<RegistrationResponse> UserRegisterAsync(RegistrationForm registrationForm)
        {
            var user = new User
            {
                Email = registrationForm.Email,
                FirstName = registrationForm.FirstName,
                LastName = registrationForm.LastName,
                UserName = registrationForm.UserName,
            };

            var result = await _userManager.CreateAsync(user, registrationForm.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return new RegistrationResponse { Errors = errors };
            }
            else
            {
                var existingUser = await _userManager.FindByNameAsync(registrationForm.UserName);

                if (existingUser != null)
                {
                    var newUserDetails = new UserDetail
                    {
                        Email = registrationForm.Email,
                        FirstName = registrationForm.FirstName,
                        LastName = registrationForm.LastName,
                        DisplayName = registrationForm.UserName,
                        UserId = existingUser.Id,
                        Description = String.Empty,
                        Status = String.Empty,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };
                    await _userDetailsService.CreateUserDetailAsync(newUserDetails);

                    await _userManager.AddToRolesAsync(existingUser, new string[] { RolesEnum.Customer });

                    if (Convert.ToBoolean(_configuration["SendGrid:IsActive"]))
                    {
                        await SendSecurityCodeAsync(user.Email);
                    }
                    else
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        await _userManager.ConfirmEmailAsync(existingUser, code);
                    }
                }
            }
            return new RegistrationResponse { IsSuccessfulRegistration = true };
        }

        public async Task<bool> ChangePasswordAsync(User user, string currentPassword, string newPassword) 
        {
            var isOldPasswordMatch = await _userManager.CheckPasswordAsync(user, currentPassword);

            if (isOldPasswordMatch)
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

                if (result.Succeeded) 
                    return true;
            }

            return false;
        }

        public async Task<bool> SendSecurityCodeAsync(string email) 
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null) 
            {
                var securityCode = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddHours(1));

                var homepageUrl = _configuration["Host"];
                var ourPage = _configuration["HomePage"];
                var firstName = user.FirstName[0].ToString().ToUpper() + user.FirstName.Substring(1).Trim();
                var message = "Dear " + firstName + ",<br><br>Please use code: <b>" + securityCode + "</b> to complete registration with our website <a href=" + homepageUrl + "><b>" + ourPage + "</b></a><br><br> Best Regards,<br>1Wallet Pro Team";

                _memoryCache.Set(user.Email, securityCode, cacheOptions);
                
                await _sendGridService.SendEmailAsync(email, message, EmailSubjectLineEnum.RegistrationSecurityCode);

                return true;
            }
            return false;
        }

        public async Task<string> CheckSecurityCodeAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null) 
            {
                var securityCodeCache = _memoryCache.Get(user.Email);

                if (securityCodeCache != null) 
                {
                    if (securityCodeCache.ToString() == code)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        await _userManager.ConfirmEmailAsync(user, token);

                        var jwtToken = await _jwtHandler.GetJwtTokenAsync(user);
                        _memoryCache.Remove(user.Email);    

                        return jwtToken;
                    }
                }
            }
            return "";
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword) 
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null) 
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded) 
                    return true;
            }

            return false;
        }

        public async Task InitAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(RolesEnum.Admin);

            if (!users.Any())
            {
                var user = new User
                {
                    Email = _configuration["Admin:Email"],
                    FirstName = _configuration["Admin:FirstName"],
                    LastName = _configuration["Admin:LastName"],
                    UserName = _configuration["Admin:UserName"],
                };

                await _userManager.CreateAsync(user, _configuration["Admin:Password"]);
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, code);

                var existingUser = await _userManager.FindByEmailAsync(_configuration["Admin:Email"]);

                if (existingUser != null)
                {
                    var newUserDetails = new UserDetail
                    {
                        Email = _configuration["Admin:Email"],
                        FirstName = _configuration["Admin:FirstName"],
                        LastName = _configuration["Admin:LastName"],
                        DisplayName = _configuration["Admin:UserName"],
                        UserId = existingUser.Id,
                        Description = String.Empty,
                        Status = "Admin",
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };

                    await _userDetailsService.CreateUserDetailAsync(newUserDetails);
                }

                List<string> roles = new List<string> { RolesEnum.Customer, RolesEnum.Admin };

                foreach (string role in roles)
                {
                    IdentityRole existRole = await _roleManager.FindByNameAsync(role);
                    if (existRole == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                await _userManager.AddToRolesAsync(user, new string[] { RolesEnum.Admin });
            }
        }
    }
}
