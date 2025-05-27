using EllipticCurve.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;
using Univisia.Finance.Data.Models;
using Univisia.Finance.Services.Services.Interfaces;
using Univisia.Finance.Web.Controllers;
using Univisia.Finance.Web.ViewModels.AccountView;
using Univisia.Finance.Web.ViewModels.LoanView;

[Authorize]
[Route("[controller]")]
public class TransactionController : BaseController
{
    private ITransactionService _transactionService;
    private readonly IUserDetailsService _userDetailsService;

    public TransactionController(ITransactionService transactionService, IUserDetailsService userDetailsService) : base(userDetailsService)
    {
        _transactionService = transactionService;
        _userDetailsService = userDetailsService;
    }

    [Route("transactions")]
    [HttpGet]
    public async Task<List<TransactionViewModel>> GetTransactionsAsync(int startIndex, int endIndex, int? accountId = null, int? loanId = null)
    {
        int existingUserId = await GetUserDetailId();
        var transactions = await _transactionService.GetAllTransactionsAsync(existingUserId, startIndex, endIndex, accountId, loanId);

        return transactions.Select(x => new TransactionViewModel(x)).ToList();
    }

    [Route("transactionscount")]
    [HttpGet]
    public async Task<int> TransactionsCountAsync(int? accountId = null, int? loanId = null)
    {
        int existingUserId = await GetUserDetailId();

        return await _transactionService.TransactionsCountAsync(existingUserId, accountId, loanId);
    }
}
