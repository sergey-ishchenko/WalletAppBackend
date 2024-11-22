namespace WalletAppBackend.Models.DTOs
{
	public class TransactionDto
	{
		
		public int Id { get; set; }
		public TransactionType Type { get; set; }
		public decimal Amount { get; set; }
		public string AmountToDisplay { get; set; } = String.Empty;
		public string Name { get; set; } = String.Empty;
		public string Description { get; set; } = String.Empty;
		public string DescriptionToDisplay { get; set; } = String.Empty;
		public DateTime Date{ get; set; }
		public string DateToDisplay { get; set; } = String.Empty;
		public TransactionStatus Status { get; set; }
		public string? LinkedUserName { get; set; }
	}
}
