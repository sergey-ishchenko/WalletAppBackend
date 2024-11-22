using Microsoft.EntityFrameworkCore;
using WalletAppBackend.Constants;
using WalletAppBackend.Data;
using WalletAppBackend.Models;

namespace WalletAppBackend.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly ApiDbContext _dbContext;

		public TransactionService(ApiDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Transaction?> GetTransaction(int transactionId)
		{
			return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
		}

		public async Task<IEnumerable<Transaction>> GetLatestUserTransactions(int userId)
		{
			return await _dbContext.Transactions
				.Where(t => t.Card.User.Id == userId)
				.Include(t => t.LinkedUser)
				.AsSplitQuery()
				.OrderByDescending(t => t.Date)
				.Take(TransactionsConsts.CountOfLatest)
				.AsNoTracking()
				.ToListAsync();
		}
	}
}
