using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Univisia.Finance.Data.Enums;
using Univisia.Finance.Services.Services.Interfaces;

namespace Univisia.Finance.Web.Controllers
{
    [Route("[controller]")]
    public class BaseController : Controller
    {
        private readonly IUserDetailsService _userDetailsService;

        public BaseController(IUserDetailsService userDetailsService)
        {
            _userDetailsService = userDetailsService;
        }

        protected async Task<int> GetUserDetailId() 
        {
            return await _userDetailsService.GetUserDetailIdByEmailAsync(User.Identity.Name);
        }

        public bool IsAdmin
        { 
            get
            {
                return User.IsInRole(RolesEnum.Admin);
            } 
        }
    }
}