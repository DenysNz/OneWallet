using Finance.Data.Models;

namespace Finance.Services.Services.Interfaces
{
    public interface ILoanService
    {
        Task AddLoanAsync(int userId, string userName, int currencyId, string person, string note, decimal amount, DateTime? deadline, string? contactEmail);

        Task ChangeStatusAsync(int loanId, int loanStatusId, string? quotes);

        Task<int> GetRequestCount(int userId);

        Task<List<Loan>> GetLoansAsync(int userId);

        Task DeleteLoanAsync(int loanId, string userName);

        Task UpdateLoanAsync(string userName, int loanId, string person, string note, decimal amount, DateTime? deadline);

        Task<Loan> GetLoanAsync(int loanId);

        Task AddLoanOperationAsync(string? userName, decimal amount, int loanId, string notes);
    }
}
