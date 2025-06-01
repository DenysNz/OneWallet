using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;
using Finance.Web.ViewModels.UserDetails;

namespace Finance.Web.Controllers
{
    [Authorize(Roles = "Customer, Admin")]
    [Route("[controller]")]
    public class UserDetailsController : BaseController
    {
        private readonly IUserDetailsService _userDetailsService;

        public UserDetailsController(IUserDetailsService userDetailsService) : base(userDetailsService)
        {
            _userDetailsService = userDetailsService;
        }

        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> GetUserDetails() 
        {
            if (User.Identity == null)
                return Forbid();

            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var existingUser = await _userDetailsService.GetUserDetailByEmailAsync(User.Identity.Name);

            if (existingUser != null) 
            {
                return Ok(new UserDetailsViewModel(existingUser));
            }

            return Forbid();
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UserDetailsViewModel userDetails) 
        {
            var existingUserId = await GetUserDetailId();

            if (User.Identity == null || existingUserId == 0)
                return Forbid();

            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var token = await _userDetailsService.UpdateUserDetailsAsync(userDetails!.ToEntity(existingUserId));

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new LoginResponse { IsAuthSuccessful = true, Token = token });
            }

            return BadRequest();
        }

        [Route("statuswelcome")]
        [HttpGet]
        public async Task<WelcomeViewModel> GetWecomePageIsViisted()
        {
            var existingUserId = await GetUserDetailId();

            var welcomeState = new WelcomeViewModel();
            welcomeState.WelcomePageIsVisited = await _userDetailsService.GetWecomePageIsViisted(existingUserId);

            return welcomeState;
        }

        [Route("updatestatuswelcome")]
        [HttpPatch]
        public async Task<IActionResult> GetGetWecomePageIsViisted()
        {
            var existingUserId = await GetUserDetailId();
            await _userDetailsService.UpdateWecomePageIsViisted(existingUserId);

            return Ok();
        }
    }
}
