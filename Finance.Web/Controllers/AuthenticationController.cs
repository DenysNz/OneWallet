using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Finance.Data.Models;
using Finance.Web.ViewModels.Authentication;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.Web.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(IAuthenticationService authenticationService,
            UserManager<User> userManager)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserFormAuthenticationViewModel userFormAuthentication)
        {
            if (userFormAuthentication == null || !ModelState.IsValid)
                return BadRequest();

            var userFrom = new LoginForm 
            {
                UserName = userFormAuthentication.UserName,
                Password = userFormAuthentication.Password
            };

            var authenticationResponse = await _authenticationService.UserLoginAsync(userFrom);
            if (!authenticationResponse.IsAuthSuccessful)
            {
                return BadRequest(authenticationResponse.ErrorMessage);
            }
            else if (authenticationResponse.IsAuthSuccessful && !string.IsNullOrEmpty(authenticationResponse.Token))
            {
                return Ok(authenticationResponse);
            }

            return BadRequest();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserFormRegistrationViewModel userFormRegistration)
        {
            if (userFormRegistration == null || !ModelState.IsValid)
                return BadRequest();

            var registrationForm = new RegistrationForm 
            {
                FirstName = userFormRegistration.FirstName,
                LastName = userFormRegistration.LastName,
                UserName = userFormRegistration.UserName,
                Email = userFormRegistration.Email,
                Password = userFormRegistration.Password,
                ConfirmPassword = userFormRegistration.ConfirmPassword
            };

            var registrationResult = await _authenticationService.UserRegisterAsync(registrationForm);

            if (!registrationResult.IsSuccessfulRegistration)
            {
                return BadRequest(registrationResult.Errors);
            }

            return Ok();
        }

        [Route("init")]
        [HttpGet]
        public async Task Init()
        {
            await _authenticationService.InitAsync();
        }

        [Route("changepassword/{email}/{currentPassword}/{newPassword}")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string email, string currentPassword, string newPassword)
        {
            var existingUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (existingUser != null && (existingUser.Email.ToLower() == email.ToLower()))
            {
                var isResultSucceeded = await _authenticationService.ChangePasswordAsync(existingUser, currentPassword, newPassword);

                if (isResultSucceeded)
                {
                    return Ok();
                }
                else 
                {
                    return BadRequest();
                }
            }

            return Forbid();
        }

        [Route("sendsecuritycode/{email}")]
        [HttpGet]
        public async Task<IActionResult> SendSecurityCode(string email)
        {
            var sendResult = await _authenticationService.SendSecurityCodeAsync(email);

            if (sendResult) 
            {
                return Ok();
            }

            return BadRequest();
        }

        [Route("verifysecuritycode/{email}/{code}")]
        [HttpGet]
        public async Task<IActionResult> CheckValidSecurityCode(string email, string code)
        {
            var token = await _authenticationService.CheckSecurityCodeAsync(email, code);

            if (!string.IsNullOrEmpty(token)) 
            {
                var data = new { Token = token };
                return Ok(data);
            }

            return BadRequest();
        }

        [Route("resetpassword/{email}/{password}")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string password)
        {     
            var isSucceeded = await _authenticationService.ResetPasswordAsync(email, password);

            if (isSucceeded)
                return Ok();

            return BadRequest();
        }
    }
}
