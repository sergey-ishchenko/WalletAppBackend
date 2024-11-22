using WalletAppBackend.Models;

namespace WalletAppBackend.Services;

public interface ITransactionService
{
	Task<Transaction?> GetTransaction(int transactionId);
	Task<IEnumerable<Transaction>> GetLatestUserTransactions(int userId);
}