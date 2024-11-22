using Microsoft.EntityFrameworkCore;
using WalletAppBackend.Data;
using WalletAppBackend.Models;

namespace WalletAppBackend.Services
{
	public class CardService : ICardService
	{
		private readonly ApiDbContext _dbContext;

		public CardService(ApiDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Card?> GetUserCard(int userId)
		{
			return await _dbContext.Cards
				.FirstOrDefaultAsync(c => c.UserId == userId);

		}
	}
}
