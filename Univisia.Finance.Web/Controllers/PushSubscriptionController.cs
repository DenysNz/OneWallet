using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Univisia.Finance.Data.Models;
using Univisia.Finance.Services.Models.Support;
using Univisia.Finance.Services.Services.Interfaces;
using Univisia.Finance.Web.Controllers;
using Univisia.Finance.Web.Filters;
using Univisia.Finance.Web.ViewModels.PushSubscription;

[Authorize]
[Route("[controller]")]
public class PushSubscriptionController : BaseController
{
    private readonly IPushSubscriptionService _servicePS;
    private readonly IUserDetailsService _userDetailsService;
    
    public PushSubscriptionController(IPushSubscriptionService servicePS, IUserDetailsService userDetailsService) : base(userDetailsService)
    {
        _servicePS = servicePS;
        _userDetailsService = userDetailsService;
    }

    [Route("pushsubscription")]
    [HttpPatch]
    public async Task<IActionResult> GetPushSubscriptionObjectAsync([FromBody] PushSubscriptionViewModel subscription)
    { 
        var existingUserId = await GetUserDetailId();

        await _servicePS.AddSubscriptionAsync(existingUserId, subscription.Endpoint, subscription.ExpirationTime, subscription.keys.EncriptionKey, subscription.keys.Secret);

        return Ok();
    }
}
