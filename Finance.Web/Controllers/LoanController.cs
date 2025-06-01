using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Finance.Data.Enums;
using Finance.Data.Models;
using Finance.Services.Services.Interfaces;
using Finance.Web.ViewModels.LoanView;
using static Finance.Services.Models.CurrencyRatesView;

namespace Finance.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class LoanController : BaseController
    {
        private ILoanService _loanService;

        public LoanController(ILoanService loanService, IUserDetailsService userDetailsService) : base(userDetailsService)
        {
            _loanService = loanService;
        }

        private async Task<bool> HasAccess(int loanId)
        {
            var existingUserId = await GetUserDetailId();
            var loan = await _loanService.GetLoanAsync(loanId);

            return existingUserId == loan.UserDetailId;
        }

        private async Task<bool> AllowedChangeStatus(int loanId)
        {
            var existingUserId = await GetUserDetailId();
            var loan = await _loanService.GetLoanAsync(loanId);

            return existingUserId == loan.ContactDetailId;
        }

        private async Task<bool> AllowedLoanOperation(int loanId)
        {
            var existingUserId = await GetUserDetailId();
            var loan = await _loanService.GetLoanAsync(loanId);

            return (existingUserId == loan.ContactDetailId || existingUserId == loan.UserDetailId);
        }

        private async Task<bool> AllowedUpdate(int loanId)
        {
            var existingUserId = await GetUserDetailId();
            var loan = await _loanService.GetLoanAsync(loanId);

            return ((existingUserId == loan.ContactDetailId || existingUserId == loan.UserDetailId) && loan.LoanStatusId != LoanStatusEnum.Approved);
        }

        [Route("loans")]
        [HttpGet]
        public async Task<List<LoanViewModel>> GetLoansAsync()
        {
            int existingUserId = await GetUserDetailId();
            var loans = await _loanService.GetLoansAsync(existingUserId);
                
            return loans
                .Select(x => new LoanViewModel(x, x.UserDetailId == existingUserId)) 
                .ToList();
        }

        [Route("update")]
        [HttpPatch]
        public async Task<IActionResult> UpdateLoanAsync([FromBody] EditLoanViewModel loanViewModel)
        {
            int existingUserId = await GetUserDetailId();

            if (IsAdmin || await AllowedUpdate(loanViewModel.LoanId))
            {
                await _loanService.UpdateLoanAsync( User.Identity.Name, loanViewModel.LoanId, loanViewModel.Person, loanViewModel.Note, loanViewModel.Amount, loanViewModel.Deadline);

                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLoanAsync([FromBody] DeleteLoanViewModel loanViewModel)
        {
            if (IsAdmin  || await HasAccess(loanViewModel.LoanId))
            {
                await _loanService.DeleteLoanAsync(loanViewModel.LoanId, User.Identity.Name);

                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        [Route("add")]
        [HttpPatch]
        public async Task<IActionResult> AddLoanAsync([FromBody] AddLoanViewModel loanViewModel)
        {
            var existingUserId = await GetUserDetailId();

            await _loanService.AddLoanAsync(existingUserId, User.Identity.Name, loanViewModel.CurrencyId, loanViewModel.Person, loanViewModel.Note, loanViewModel.Amount, loanViewModel.Deadline, loanViewModel.ContactEmail);

            return Ok();
        }

        [Route("operation")]
        [HttpPatch]
        public async Task<IActionResult> AddLoanOperation([FromBody] LoanOperationViewModel operationViewModel)
        {
            if (IsAdmin || await AllowedLoanOperation(operationViewModel.LoanId))
            {
                await _loanService.AddLoanOperationAsync(
                  User.Identity.Name
                , operationViewModel.Amount
                , operationViewModel.LoanId
                , operationViewModel.Notes);
            }
            return Ok();
        }

        [Route("approveloan")]
        [HttpPatch]
        public async Task<IActionResult> ApproveLoan([FromBody] ChangeLoanStatus changeLoanStatus)
        {
            if (IsAdmin || await AllowedChangeStatus(changeLoanStatus.LoanId))
            {
                await _loanService.ChangeStatusAsync(changeLoanStatus.LoanId, LoanStatusEnum.Approved, changeLoanStatus.Quotes);

                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        [Route("rejectloan")]
        [HttpPatch]
        public async Task<IActionResult> RejectLoan([FromBody] ChangeLoanStatus changeLoanStatus)
        {
            if (IsAdmin || await AllowedChangeStatus(changeLoanStatus.LoanId))
            {
                await _loanService.ChangeStatusAsync(changeLoanStatus.LoanId, LoanStatusEnum.Rejected, changeLoanStatus.Quotes);

                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        [Route("getrequestcount")]
        [HttpGet]
        public async Task<int> GetRequestCounts()
        {
            var existingUserId = await GetUserDetailId();

            return await _loanService.GetRequestCount(existingUserId);
        }
    }
}
