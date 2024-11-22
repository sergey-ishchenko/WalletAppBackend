using Microsoft.EntityFrameworkCore;
using WalletAppBackend.Models;

namespace WalletAppBackend.Data;

public class ApiDbContext: DbContext
{
	public ApiDbContext(DbContextOptions<ApiDbContext> options)
	: base(options)
	{ }

	public DbSet<User> Users { get; set; }
	public DbSet<Card> Cards { get; set; }
	public DbSet<Transaction> Transactions{ get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Transaction>()
			.Property(e => e.Type)
			.HasConversion<string>();

		modelBuilder
			.Entity<Transaction>()
			.Property(e => e.Status)
			.HasConversion<string>();

		modelBuilder
			.Entity<Transaction>()
			.Property(e => e.Date)
			.HasConversion(
				d => d.ToUniversalTime(),
				d => d.ToLocalTime());
	}
}
