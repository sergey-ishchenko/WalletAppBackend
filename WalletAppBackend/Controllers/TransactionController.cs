using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using WalletAppBackend.Constants;
using WalletAppBackend.Models;
using WalletAppBackend.Models.DTOs;
using WalletAppBackend.Services;

namespace WalletAppBackend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TransactionController : Controller
	{
		private readonly ITransactionService _transactionService;
		private readonly ICardService _cardService;

		public TransactionController(
			ITransactionService transactionService,
			ICardService cardService)
		{
			_transactionService = transactionService;
			_cardService = cardService;
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> GetTransactions(int userId)
		{
			var userCard = await _cardService.GetUserCard(userId);
			if (userCard is null)
				return BadRequest("User with this ID doesn't have any card!");


			var dailyPoints = CalculateDailyPoints();

			var resultDto = new UserTransactionsCardDto()
			{
				CardBalance = userCard.Balance,
				AvailableFunds = CardConsts.Limit - userCard.Balance,
				DailyPoints = dailyPoints,
				FormatedDailyPoints = FormatPoint(dailyPoints),         //Should be generated on frontend
				NoPaymentDueText = $"You’ve paid your {DateTime.Today:MMMM} balance.",
			};

			var transactions = await _transactionService.GetLatestUserTransactions(userId);

			var transactionDtoList = new List<TransactionDto>();
			foreach (var transaction in transactions)
			{
				var transactionDto = TransformTransactionsToDto(transaction);
				transactionDtoList.Add(transactionDto);
			}

			resultDto.LatestTransactions = transactionDtoList;

			return Ok(resultDto);
		}

		[HttpGet]
		[Route("[action]")]
		public async Task<ActionResult> GetTransactionDetail(int transactionId)
		{
			var transaction = await _transactionService.GetTransaction(transactionId);
			if (transaction is null)
				return BadRequest("Transaction not found.");

			var transactionDto = TransformTransactionsToDto(transaction);

			return Ok(transactionDto);
		}

		private TransactionDto TransformTransactionsToDto(Transaction transaction)
		{
			string amountSign = transaction.Type == TransactionType.Payment ? "+" : string.Empty;
			string descriptionPrefix = transaction.Status == TransactionStatus.Pending ? "Pending - " : string.Empty;
			string datePrefix = transaction.LinkedUser is not null
				? $"{transaction.LinkedUser.Name} - "
				: String.Empty;

			var transactionDto = new TransactionDto()
			{
				Id = transaction.Id,
				Amount = transaction.Amount,
				AmountToDisplay = $"{amountSign}${transaction.Amount:0.00}", //Should be generated on frontend
				Name = transaction.Name,
				Description = transaction.Description,
				DescriptionToDisplay =
					$"{descriptionPrefix}{transaction.Description}", //Should be generated on frontend
				Status = transaction.Status,
				Type = transaction.Type,
				LinkedUserName = transaction.LinkedUser?.Name,
				Date = transaction.Date,
				DateToDisplay = $"{datePrefix}{NormalizedDate(transaction.Date)}" //Should be generated on frontend
			};

			return transactionDto;
		}

		private string NormalizedDate(DateTime date)
		{
			var timeSpan = DateTime.Today - date.Date;
			return timeSpan.Days switch
			{
				0 => "Today",
				1 => "Yesterday",
				> 1 and < 7 => date.DayOfWeek.ToString(),
				_ => date.ToString("MM\\/dd\\/yy")
			};
		}

		private double CalculateDailyPoints()
		{
			var today = DateTime.Today;
			DateTime currentSeasonStart = GetSeasonStartDate(today);

			var daysOfSeason = (today - currentSeasonStart).Days + 1;

			var points = Calculate(1, 0, 0, 0);

			double Calculate(int day, double points, double prevPoints, double beforePrevPoints)
			{
				var dailyPoints = 0.0;

				if (day == 1)
				{
					dailyPoints += 2.0;
				}
				else if (day == 2)
					dailyPoints += 3.0;
				else
					dailyPoints = beforePrevPoints + (prevPoints * 0.6);

				if (day == daysOfSeason)
					return dailyPoints;

				points += dailyPoints;
				day++;

				return Calculate(day, points, dailyPoints, prevPoints);
			}

			return points;
		}

		private DateTime GetSeasonStartDate(DateTime seasonDate)
		{
			return seasonDate switch
			{
				{ Month: 12 or 1 or 2 } date => new DateTime(date.Month == 12 ? date.Year : date.Year - 1, 12, 1),
				{ Month: >= 3 and <= 5 } date => new DateTime(date.Year, 3, 1),
				{ Month: >= 6 and <= 8 } date => new DateTime(date.Year, 6, 1),
				var date => new DateTime(date.Year, 9, 1),
			};
		}

		private string FormatPoint(double value)
		{
			var integerCount = Math.Floor(Math.Log10(value) + 1);

			string formatedValue = "";

			if (integerCount is >= 4 and <= 6)
			{
				formatedValue = $"{Math.Round(value / 1_000)}K";
			}
			else if (integerCount is >= 7 and <= 9)
			{
				formatedValue = $"{Math.Round(value / 1_000_000)}M";
			}
			else if (integerCount is >= 10 and <= 12)
			{
				formatedValue = $"{Math.Round(value / 1_000_000_000)}B";
			}
			else if (integerCount is >= 13 and <= 15)
			{
				formatedValue = $"{Math.Round(value / 1_000_000_000_000)}T";
			}
			else
				formatedValue = value.ToString();

			return formatedValue;
		}

		private string FormatPoint2(double value)
		{
			var integerCount = Math.Floor(Math.Log10(value) + 1);

			string formatedValue = integerCount switch
			{
				>= 4 and <= 6 => $"{Math.Round(value / 1_000)}K",
				>= 7 and <= 9 => $"{Math.Round(value / 1_000_000)}M",
				>= 10 and <= 12 => $"{Math.Round(value / 1_000_000_000)}B",
				>= 13 and <= 15 => $"{Math.Round(value / 1_000_000_000_000)}T",
				_ => value.ToString(CultureInfo.InvariantCulture)
			};

			return formatedValue;
		}
	}
}
