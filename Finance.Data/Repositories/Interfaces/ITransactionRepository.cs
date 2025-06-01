using Finance.Data.Models;

namespace Finance.Data.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<List<Transaction>> GetAllTransactionsAsync(int userId, int startIndex, int endIndex, int? accountId, int? loanId);

        Task<IQueryable<Transaction>> GetTarnsactionsAsync(int userId, int? accountId, int? loanId);

        Task<int> TransactionsCountAsync(int userId, int? accountId, int? loanId);
    }
}
