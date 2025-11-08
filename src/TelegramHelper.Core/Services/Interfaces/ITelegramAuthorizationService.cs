namespace TelegramHelper.Core.Services.Interfaces;

public interface ITelegramAuthorizationService
{
	Task SendCodeAsync(string code);
	Task SendPasswordAsync(string password);
}
