using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Data.Models;
using Finance.Services.Models.Support;
using Finance.Services.Services.Interfaces;
using Finance.Web.Controllers;
using Finance.Web.Filters;
using Finance.Web.ViewModels.PushSubscription;

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
