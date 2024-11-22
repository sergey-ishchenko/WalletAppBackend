using Microsoft.EntityFrameworkCore;
using WalletAppBackend.Data;

namespace WalletAppBackend.Extensions
{
	public static class MigrationExtensions
	{
		public static async Task ApplyMigrations(this IApplicationBuilder app, ILogger logger)
		{
			using IServiceScope scope = app.ApplicationServices.CreateScope();

			await using ApiDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();

			var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
			if (pendingMigrations.Any())
			{
				await dbContext.Database.MigrateAsync();
				logger.LogDebug("Migrations were applied.");
			}
		}
	}
}
