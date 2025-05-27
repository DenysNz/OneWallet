using Microsoft.AspNetCore.Mvc;
using Univisia.Finance.Services.Models.Support;
using Univisia.Finance.Services.Services.Interfaces;
using Univisia.Finance.Web.Controllers;
using Univisia.Finance.Web.Filters;

[CaptchaActionFilter]
[Route("[controller]")]
public class SupportController : BaseController
{
    private readonly ISupportService _supportService;
    private readonly IUserDetailsService _userDetailsService;

    public SupportController(ISupportService supportService, IUserDetailsService userDetailsService) : base(userDetailsService)
    {
        _supportService = supportService;
        _userDetailsService = userDetailsService;
    }

    [Route("requestsupport")]
    [HttpPatch]
    public async Task<IActionResult> CreateRequestAsync([FromBody] SupportRequestViewModel request)
    {
        var existingUserId = await GetUserDetailId();

        await _supportService.AddRequestAsync(existingUserId, request.Name, request.Email, request.Text);

        return Ok();
    }
}
