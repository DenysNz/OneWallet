using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using Finance.Data.Enums;
using Finance.Data.Models;
using Finance.Data.Repositories.Interfaces;

namespace Finance.Data.Repositories
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        protected readonly FinanceDbContext _dbContext;
        public LoanRepository( FinanceDbContext dbContext) : base( dbContext ) 
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddLoanAsync(int userId, string userName, int currencyId, string person, string note, decimal amount, DateTime? deadline, string? contactMail)
        {
            var contacId = -1;

            var loan = new Loan()
            {
                UserDetailId = userId,
                CreatedBy = userName,
                CurrencyId = currencyId,
                Person = person,
                Note = note,
                Amount = amount,
                Deadline = deadline,
                CreatedDate = DateTime.UtcNow,
            };

            if (!String.IsNullOrEmpty(contactMail))
            {
                loan.ContactEmail = contactMail.Trim();

                var contact = await _dbContext
               .UserDetails
               .Where(x => x.Email == contactMail)
               .FirstOrDefaultAsync();

                if ( contact != null )
                {
                    loan.ContactDetailId = contact.UserDetailId;
                    contacId = contact.UserDetailId;
                }
                else
                {
                    contacId = 0;
                }
            }

            loan.LoanStatusId = String.IsNullOrEmpty(contactMail) ? LoanStatusEnum.Private : LoanStatusEnum.Requested;

            await _dbContext.Loans.AddAsync(loan);
            await _dbContext.SaveChangesAsync();

            return contacId;
        }

        public async Task ChangeStatusAsync(int loanId,  int loanStatusId, string? quotes)
        {
            var loan = await _dbContext.FindAsync<Loan>(loanId);

            var contact = _dbContext
                         .UserDetails
                         .Where(x => x.Email == loan.ContactEmail)
                         .FirstOrDefault();
            
            loan.LoanStatusId = loanStatusId;
            loan.UpdatedDate = DateTime.UtcNow;
            loan.UpdatedBy = contact.Email;

            var transaction = new Transaction
            {
                LoanId = loanId,
                CreatedBy = contact.Email,
                CreatedDate = DateTime.UtcNow,
                QuoteOnStatus = quotes,
            };
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<int> GetRequestCount(int userId)
        {
            return await _dbContext
                .Loans
                .Where(x => x.ContactDetailId == userId && x.LoanStatusId == LoanStatusEnum.Requested && !x.IsDeleted)
                .CountAsync();
        }

        public async Task<List<Loan>> GetAllLoansAsync(int userId)
        {
            return await _dbContext
                .Loans
                .Include(x => x.Currency)
                .Include(x => x.LoanStatus)
                .Where(x => (x.UserDetailId == userId || (x.ContactDetailId == userId && !(x.LoanStatusId == LoanStatusEnum.Rejected))) && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<Loan> DeleteLoanByIdAsync(int loanId, string userName)
        {
            var deleteLoan = await _dbContext
                .Loans
                .Include(x => x.Currency)
                .Include(x => x.UserDetail)
                .Include(x => x.ContactDetail)
                .Where(x => x.LoanId == loanId)
                .FirstAsync();
            
            deleteLoan.UpdatedBy = userName;
            deleteLoan.UpdatedDate = DateTime.UtcNow;
            deleteLoan.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return deleteLoan;
        }

        public async Task UpdateLoanByIdAsync(string userName, int loanId, string person, string note, decimal amount, DateTime? deadline)
        {
            var updateLoan = await _dbContext.FindAsync<Loan>(loanId);
            
            updateLoan.Amount = amount;
            updateLoan.Person = person;
            updateLoan.Deadline = deadline;
            updateLoan.Note = note;
            updateLoan.UpdatedBy = userName;
            updateLoan.UpdatedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Loan> AddLoanOperationAsync(int loanId, string userName, decimal amount, string notes)
        {
            var loan = await _dbContext.Loans
                .Include(x => x.UserDetail)
                .Include(x => x.ContactDetail)
                .Include(x => x.Currency)
                .Where(x => x.LoanId == loanId)
                .FirstAsync();
                
            var contactEmail = loan.ContactEmail?.Trim();
            var userEmail = loan.UserDetail.Email.Trim();

            loan.Amount += amount;
            loan.UpdatedDate = DateTime.UtcNow;
            loan.UpdatedBy = userName;

            var transaction = new Transaction
            {
                LoanId = loanId,
                CreatedBy = userName,
                CreatedDate = DateTime.UtcNow,
                TransactionAmount = amount,
                Notes = notes
            };

            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            return loan;
        }

        public async Task<Loan?> GetLoanByIdAsync(int loanId)
        {
            return await _dbContext.FindAsync<Loan>(loanId);
        }
    }
}
