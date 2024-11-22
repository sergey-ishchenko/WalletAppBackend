namespace WalletAppBackend.Models;

public sealed class Transaction
{
	public int Id { get; set; }
	public TransactionType Type { get; set; }
	public decimal Amount { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public DateTime Date { get; set; }
	public TransactionStatus Status { get; set; }
	public int? LinkedUserId { get; set; }
	public User? LinkedUser { get; set; } = null!;

	public int CardId { get; set; }
	public Card Card { get; set; } = null!;
}

public enum TransactionType
{
	Credit,
	Payment
}

public enum TransactionStatus
{
	Pending,
	Approved
}