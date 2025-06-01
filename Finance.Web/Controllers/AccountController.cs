using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Finance.Services.Models;
using Finance.Services.Services.Interfaces;
using Finance.Web.Controllers;
using Finance.Web.ViewModels.AccountView;

[Authorize]
[Route("[controller]")]
public class AccountController : BaseController
{
    private IBankAccountService _accountService;
    private readonly IUserDetailsService _userDetailsService;

    public AccountController(IBankAccountService accountService, IUserDetailsService userDetailsService) : base(userDetailsService)
    {
        _accountService = accountService;
        _userDetailsService = userDetailsService;
    }

    private async Task<bool> HasAccess(int accountId)
    {
        var existingUserId = await GetUserDetailId();
        var account = await _accountService.GetAccountAsync(accountId);

        return existingUserId == account.UserDetailId;
    }

    [Route("accounts")]
    [HttpGet]
    public async Task<List<AccountViewModel>> GetAccountsAsync(bool includeDeleted = false)
    {
        int existingUserId = await GetUserDetailId();
        var accounts = await _accountService.GetAllAccountsAsync(existingUserId, includeDeleted);
       
            return accounts
                .Select(x => new AccountViewModel(x))
                .ToList();
    }

    [Route("add")]
    [HttpPatch]
    public async Task<IActionResult> AddAccountAsync([FromBody] AccountViewModel accountViewModel)
    {
        var existingUserId = await GetUserDetailId();

        await _accountService.AddAccountAsync(existingUserId, User.Identity.Name, accountViewModel.CurrencyId, accountViewModel.Name, accountViewModel.Amount);

        return Ok();
    }

    [Route("delete")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAccountAsync([FromBody] DeleteAccountViewModel accountViewModel)
    {
        if (IsAdmin || await HasAccess(accountViewModel.AccountId))
        {
            await _accountService.DeleteAccountAsync(accountViewModel.AccountId, User.Identity.Name);

            return Ok();
        }
        else
        {
            return Forbid();
        }
    }

    [Route("changebalance")]
    [HttpPatch]
    public async Task<IActionResult> ChangeBalanceAsync([FromBody] ChangeBalanceViewModel accountViewModel)
    {
        if (IsAdmin || await HasAccess(accountViewModel.AccountId))
        {
            await _accountService.ChangeBalanceAsync(accountViewModel.AccountId, User.Identity.Name, accountViewModel.Amount, accountViewModel.Notes);
            
            return Ok();
        }
        else
        {
            return Forbid();
        }
    }

    [Route("update")]
    [HttpPatch]
    public async Task<IActionResult> EditAccountAsync([FromBody] EditAccountViewModel editViewModel)
    {
        if(IsAdmin || await HasAccess(editViewModel.AccountId))
        {
            await _accountService.UpdateAccountAsync(editViewModel.AccountId, User.Identity.Name, editViewModel.Name, editViewModel.CurrencyId);

            return Ok();
        }
        else
        {
            return Forbid();
        }
    }

    [Route("balance")]
    [HttpGet]
    public async Task<List<Balance>> GetBalanceAsync()
    {
        int existingUserId = await GetUserDetailId();

        return await _accountService.GetBalanceAsync(existingUserId);
    }

    [Route("lookup")]
    [HttpGet]
    public async Task<List<LookupItem>> AccountsLookup()
    {
        int existingUserId = await GetUserDetailId();

        return await _accountService.AccountsLookup(existingUserId);
    }

    [Route("wallet")]
    [HttpGet]
    public async Task<BalanceViewModel> GetWalletAsync()
    {
        int existingUserId = await GetUserDetailId();

        var balanceViewModel = new BalanceViewModel();
        balanceViewModel.Wallets = await _accountService.GetWalletAsync(existingUserId);

        try
        {
            balanceViewModel.Balance = await _accountService.CalculateTotal(balanceViewModel.Wallets);
        }
        catch(Exception ex)
        {
            //TODO: log
        }

        return balanceViewModel;  
    }
}
