namespace WalletAppBackend.Models;

public sealed class Card
{
	public int Id { get; set; }
	public decimal Balance { get; set; }
	public int UserId { get; set; }
	public User User { get; set; } = null!;

	public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

}
