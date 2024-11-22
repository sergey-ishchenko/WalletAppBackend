namespace WalletAppBackend.Models.DTOs
{
	public class UserTransactionsCardDto
	{
		public decimal CardBalance { get; set; }
		public decimal AvailableFunds { get; set; }
		public string NoPaymentDueText { get; set; } = String.Empty;
		public double DailyPoints { get; set; }
		public string FormatedDailyPoints { get; set; } = String.Empty;
		public IEnumerable<TransactionDto> LatestTransactions { get; set; } = Enumerable.Empty<TransactionDto>();
	}
}
