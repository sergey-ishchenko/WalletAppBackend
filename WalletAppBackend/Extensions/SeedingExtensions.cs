using Microsoft.EntityFrameworkCore;
using WalletAppBackend.Data;
using WalletAppBackend.Data.Seed;

namespace WalletAppBackend.Extensions
{
    public static class SeedingExtensions
	{
		public static async Task ApplySeeding(this IApplicationBuilder app, ILogger logger)
		{
			using IServiceScope scope = app.ApplicationServices.CreateScope();
			
			var scopedProvider = scope.ServiceProvider;

			using var apiDbContext = scopedProvider.GetRequiredService<ApiDbContext>();

			await DbSeeding.Seed(apiDbContext, logger);

		}
	}
}
