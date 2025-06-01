using Microsoft.AspNetCore.Mvc;
using Finance.Web.ViewModels.Social;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.web.Controllers
{
    [Route("[controller]")]
    public class SocialController : Controller
    {
        private readonly ISocialService _socialService;

        public SocialController(ISocialService socialService)
        {
            _socialService = socialService;
        }

        [Route("googlesettings")]
        [HttpGet]
        public IActionResult GetGoogleSettings() 
        {
            var googleSettings = new GoogleSettingViewModel
            {
                ClientId = _socialService.GetGoogleSettings()
            };

            return Ok(googleSettings);
        }

        [Route("googlelogin")]
        [HttpPost]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginViewModel googleLoginModel) 
        {
            var result = await _socialService.GoogleLoginAsync(googleLoginModel.Credential);

            if (result.IsAuthSuccessful && string.IsNullOrEmpty(result.ErrorMessage)) 
            {
                return Ok(result);
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage)) 
            {
                return BadRequest(result);
            }

            return Forbid();
        }

        [Route("socialnetworklogin")]
        [HttpPost]
        public async Task<IActionResult> SocialNetworkLogin([FromBody] SocialNetworkViewModel loginViewModel)
        {
            var socialUserDetails = new SocialNetworkUserDetails {
                FirstName = loginViewModel.FirstName,
                LastName = loginViewModel.LastName,
                Email = loginViewModel.Email,
                UserName = loginViewModel.UserName,
                AvatarUrl = loginViewModel.AvatarUrl
            };
            var result = await _socialService.LoginOrRegisterUserBySocialNetworks(socialUserDetails);

            if (result.IsAuthSuccessful && string.IsNullOrEmpty(result.ErrorMessage))
            {
                return Ok(result);
            }
            else if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(result);
            }

            return Forbid();
        }
    }
}
