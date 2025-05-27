using Univisia.Finance.Data.Models;
using Univisia.Finance.Services.Services.Interfaces;
using Univisia.Finance.Data.Repositories.Interfaces;

namespace Univisia.Finance.Services
{
    public class TransactionService : ITransactionService 
    {
        private ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync(int userId, int startIndex, int endIndex, int? accountId, int? loanId)
        {
            return await _transactionRepository.GetAllTransactionsAsync(userId, startIndex, endIndex, accountId, loanId);
        }

        public async Task<int> TransactionsCountAsync(int userId, int? accountId, int? loanId)
        {
            return await _transactionRepository.TransactionsCountAsync(userId, accountId, loanId);
        }
    }
}