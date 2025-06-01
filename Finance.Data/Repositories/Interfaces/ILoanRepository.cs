using Finance.Data.Models;

namespace Finance.Data.Repositories.Interfaces
{
    public interface ILoanRepository : IGenericRepository<Loan>
    {
        Task<int> AddLoanAsync(int userId, string userName, int currencyId, string person, string note, decimal amount, DateTime? deadline, string? contactEmail);

        Task ChangeStatusAsync(int loanId, int loanStatusId, string? quotes);

        Task<int> GetRequestCount(int userId);

        Task<List<Loan>> GetAllLoansAsync(int userId);

        Task<Loan> DeleteLoanByIdAsync(int loanId, string userName);

        Task UpdateLoanByIdAsync(string userName, int loanId, string person, string note, decimal amount, DateTime? deadline);

        Task<Loan> GetLoanByIdAsync(int loanId);

        Task<Loan> AddLoanOperationAsync(int loanId, string userName, decimal amount, string notes);
    }
}
