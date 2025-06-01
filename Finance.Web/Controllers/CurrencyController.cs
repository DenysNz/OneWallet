using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Finance.Data.Models;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;

namespace Finance.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class CurrencyController : BaseController
    {
        private ICurrencyService _currencyService;
        private readonly IUserDetailsService _userDetailsService;

        public CurrencyController(ICurrencyService currencyService, IUserDetailsService userDetailsService) : base(userDetailsService)
        {
            _currencyService = currencyService;
            _userDetailsService = userDetailsService;
        }

        [Route("lookup")]
        [HttpGet]
        public async Task<List<CurrencyModel>> CurrencyLookup(bool justPopular = false)
        {
            return await _currencyService.CurrencyLookup(justPopular);
        }

        [Route("usercurrencies")]
        [HttpGet]
        public async Task<List<LookupItem>> GetUserCurrencies()
        {
            var existingUserId = await GetUserDetailId();

            return  await _currencyService.GetUserCurrencies(existingUserId);
        }

        [Route("saveusercurrencies")]
        [HttpPatch]
        public async Task<IActionResult> SaveUserCurrencies([FromBody] List<LookupItem> currencies)
        {
            var existingUserId = await GetUserDetailId();

            await _currencyService.SaveUserCurrencies(currencies, existingUserId);

            return Ok();
        }

        //[Route("rates")]
        //[HttpGet]
        //public async Task<ActionResult<decimal>> GetRate(string fromCurrency, string toCurrency)
        //{
        //    var rate = await _currencyService.GetRate(fromCurrency, toCurrency);

        //    return Ok(rate);
        //}
    }
}
