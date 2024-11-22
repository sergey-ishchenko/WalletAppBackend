using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WalletAppBackend.Constants;
using WalletAppBackend.Models;

namespace WalletAppBackend.Data.Seed
{
	public class DbSeeding
	{
		public static async Task  Seed(
				ApiDbContext dbContext,
				ILogger logger)
		{
			try
			{
				var transaction = await dbContext.Database.BeginTransactionAsync();
				await AddUsers(dbContext, logger);
				await AddCards(dbContext, logger);
				await AddTransactions(dbContext, logger);

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				logger.LogError($"An error occurred seeding the DB. {ex.Message}");
			}
		}

		private static async Task AddUsers(ApiDbContext dbContext, ILogger logger)
		{
			if (!await dbContext.Users.AnyAsync())
			{
				IEnumerable<User> users = new List<User>()
					{
						new()
						{
							Id = 1,
							Name = "Alex",
							Email = "alex@mail.com"
						},
						new()
						{
							Id = 2,
							Name = "Helena",
							Email = "lux@mail.com"
						}
					};
				
			 	await dbContext.Users.AddRangeAsync(users);
			    await dbContext.SaveChangesAsync();
				logger.LogDebug("Users has been added.");
			}
		}

		private static async Task AddCards(ApiDbContext dbContext, ILogger logger)
		{
			if (!await dbContext.Cards.AnyAsync())
			{
				Random rnd = new Random();
				IEnumerable<Card> cards = new List<Card>()
				{
					new()
					{
						Id = 1,
						Balance = rnd.Next(0, (int)CardConsts.Limit),
						UserId = 1
					},
					new()
					{
						Id = 2,
						Balance = rnd.Next(0, (int)CardConsts.Limit),
						UserId = 2
					}
				};

				await dbContext.Cards.AddRangeAsync(cards);
				await dbContext.SaveChangesAsync();
				logger.LogDebug("Cards has been added.");
			}
		}

		private static async Task AddTransactions(ApiDbContext dbContext, ILogger logger)
		{
			if (!await dbContext.Transactions.AnyAsync())
			{

				IEnumerable<Transaction> transactions = new List<Transaction>()
				{
					new()
					{
						Type = TransactionType.Credit,
						Amount = 14.06M,
						Name = "Apple",
						Description = "Card Number Used",
						Date = DateTime.UtcNow.AddHours(-2),
						Status = TransactionStatus.Pending,
						LinkedUserId = 2,
						CardId = 1,
					},
					new()
					{
						Type = TransactionType.Payment,
						Amount = 174M,
						Name = "Payment",
						Description = "From JPMorgan Chase Bank National Association",
						Date = DateTime.UtcNow.AddDays(-1),
						Status = TransactionStatus.Approved,
						CardId = 1,
					},
					new()
					{
						Type = TransactionType.Credit,
						Amount = 3.24M,
						Name = "Apple",
						Description = "Card Number Used",
						Date = DateTime.UtcNow.AddDays(-2),
						Status = TransactionStatus.Approved,
						LinkedUserId = 2,
						CardId = 1,
					},
					new()
					{
						Type = TransactionType.Payment,
						Amount = 99.71M,
						Name = "Payment",
						Description = "From JPMorgan Chase Bank National Association",
						Date = DateTime.UtcNow.AddDays(-5),
						Status = TransactionStatus.Approved,
						CardId = 1,
					},
					new()
					{
						Type = TransactionType.Payment,
						Amount = 73.58M,
						Name = "Payment",
						Description = "From JPMorgan Chase Bank National Association",
						Date = DateTime.UtcNow.AddDays(-7),
						Status = TransactionStatus.Approved,
						CardId = 1,
					},
					new()
					{
						Type = TransactionType.Credit,
						Amount = 21.61M,
						Name = "IKEA",
						Description = "Round Rock, TX",
						Date = DateTime.UtcNow.AddDays(-8),
						Status = TransactionStatus.Approved,
						CardId = 1,
					},
					new()
					{
						Type = TransactionType.Credit,
						Amount = 73.58M,
						Name = "Target",
						Description = "Cedar Park, TX",
						Date = DateTime.UtcNow.AddDays(-10),
						Status = TransactionStatus.Approved,
						CardId = 1,
					},
				};

				await dbContext.Transactions.AddRangeAsync(transactions);
				await dbContext.SaveChangesAsync();
				logger.LogDebug("Transactions has been added.");
			}
		}
	}
}
