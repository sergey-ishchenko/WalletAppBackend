using WalletAppBackend.Models;

namespace WalletAppBackend.Services;

public interface ICardService
{
	Task<Card?> GetUserCard(int userId);
}