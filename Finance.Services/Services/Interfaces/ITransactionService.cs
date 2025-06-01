using Finance.Data.Models;

namespace Finance.Services.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetAllTransactionsAsync(int userId, int startIndex, int endIndex, int? accountId, int? loanId);

        Task<int> TransactionsCountAsync(int userId, int? accountId, int? loanId);
    }
}
