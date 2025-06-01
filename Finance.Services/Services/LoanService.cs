using Finance.Data.Models;
using Finance.Services.Services.Interfaces;
using Finance.Data.Repositories.Interfaces;
using Finance.Data.Enums;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace Finance.Services
{
    public class LoanService : ILoanService
    {
        private ILoanRepository _repositoryLoan;
        private ISendGridService _sendGridService;
        private IPushSubscriptionService _pushSubscriptionService;
        private readonly IConfiguration _configuration;

        public LoanService(ILoanRepository repositoryLoan, ISendGridService sendGridService, IPushSubscriptionService pushSubscriptionService, IConfiguration configuration)
        {
            _repositoryLoan = repositoryLoan;
            _sendGridService = sendGridService;
            _pushSubscriptionService = pushSubscriptionService;
            _configuration = configuration;
        }

        public async Task AddLoanAsync(int userId,  string userName, int currencyId, string person, string note, decimal amount, DateTime? deadline, string? contactEmail)
        {
            var contactId = await _repositoryLoan.AddLoanAsync(userId, userName, currencyId, person, note, amount, deadline, contactEmail);
            var homepageUrl = _configuration["Host"];
            var ourPage = _configuration["HomePage"];

            var messageToUnregistered = "Dear future user,<br><br>You have a loan request recorded on our site.<br>Please register or install PWA application using recorded email <b>" + contactEmail + "</b> by visiting <a href=" + homepageUrl + "><b>" + ourPage + "</b></a><br><br> Best Regards,<br>1Wallet Pro Team";
            var messageToRegistered = "Dear user,<br><br>You have a new loan request, which you could approve or deny.<br>Please visit <a href=" + homepageUrl + "><b>" + ourPage + "</b></a> to gain more insight or install PWA application.<br><br>Best Regards,<br>1Wallet Pro Team";

            if (contactId == 0)
            {
                await _sendGridService.SendEmailAsync(contactEmail, messageToUnregistered, EmailSubjectLineEnum.LoanRequest);

                return;
            }
            else if (contactId != -1)
            {
                //sending Push Subscription notification might be implemented in the future
                //for now we will use send email notification option instead
                //await _pushSubscriptionService.PushAll(contactId);

                await _sendGridService.SendEmailAsync(contactEmail, messageToRegistered, EmailSubjectLineEnum.LoanRequest);

                return;
            }

            return;
        }

        public async Task ChangeStatusAsync(int loanId, int loanStatusId, string? quotes)
        {
            await _repositoryLoan.ChangeStatusAsync(loanId, loanStatusId, quotes);
        }

        public async Task<int> GetRequestCount(int userId)
        {
            return await _repositoryLoan.GetRequestCount(userId);
        }

        public async Task<List<Loan>> GetLoansAsync(int userId)
        {
            return await _repositoryLoan.GetAllLoansAsync(userId);
        }

        public async Task DeleteLoanAsync(int loanId, string userName)
        {
            var deletedLoan = await _repositoryLoan.DeleteLoanByIdAsync(loanId, userName);
            var contactEmail = deletedLoan.ContactEmail;
            var toName = (deletedLoan.ContactDetail != null) ?
                deletedLoan.ContactDetail.FirstName[0].ToString().ToUpper() + deletedLoan.ContactDetail.FirstName.Substring(1).Trim() : "user";
            var fromName = deletedLoan.UserDetail.FirstName[0].ToString().ToUpper() + deletedLoan.UserDetail.FirstName.Substring(1).Trim();
            var amount = deletedLoan.Amount;
            var currencyName = deletedLoan.Currency.CurrencyName;
            var homepageUrl = _configuration["Host"];
            var ourPage = _configuration["HomePage"];

            var messageToContact = "Dear " + toName + ",<br><br>" + fromName + " deleted loan in amount: <b>" + amount + "</b><b> " + currencyName + "</b> on our website <a href=" + homepageUrl + "><b>" + ourPage + "</b></a><br><br>Best Regards,<br>1Wallet Pro Team";

            await _sendGridService.SendEmailAsync(contactEmail, messageToContact, EmailSubjectLineEnum.LoanDeleted);

        }

        public async Task UpdateLoanAsync(string userName, int loanId, string person, string note, decimal amount, DateTime? deadline)
        {
            await _repositoryLoan.UpdateLoanByIdAsync(userName, loanId, person, note, amount, deadline);
        }

        public async Task<Loan> GetLoanAsync(int loanId)
        {
            return await _repositoryLoan.GetLoanByIdAsync(loanId);
        }

        public async Task AddLoanOperationAsync(string? userName, decimal amount, int loanId, string notes)
        {
            var loan  = await _repositoryLoan.AddLoanOperationAsync(loanId, userName, amount, notes);
            var fromUser = loan.UserDetail.FirstName[0].ToString().ToUpper() + loan.UserDetail.FirstName.Substring(1).Trim();
            var toContact = (loan.ContactDetail != null) ?
                loan.ContactDetail.FirstName[0].ToString().ToUpper() + loan.ContactDetail.FirstName.Substring(1).Trim() : "future user";
            var currencyName = loan.Currency.CurrencyName;

            var message = "Dear " + toContact + ",<br><br>" + fromUser + " added new loan transaction: Amount: <b>" + amount + "</b><b> " + currencyName + "</b> with Note: <b>" + notes + "</b>.<br><br>Best Regards,<br> 1Wallet Pro Team";

            var homepageUrl = _configuration["Host"];

            if (!String.IsNullOrEmpty(loan.ContactEmail))   
            {
                await _sendGridService.SendEmailAsync(loan.ContactEmail, message, EmailSubjectLineEnum.NewLoanTransaction);
            }
        }
    }
}   