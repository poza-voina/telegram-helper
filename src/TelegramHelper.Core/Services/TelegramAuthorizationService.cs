using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TdLib;
using TelegramHelper.Core.ObjectStorage.LogObjects;
using TelegramHelper.Core.Services.Interfaces;
using static TdLib.TdApi;

namespace TelegramHelper.Core.Services;

public class TelegramAuthorizationService : ITelegramAuthorizationService
{
	private readonly TdClient _tdClient;
	private readonly ILogger<TelegramAuthorizationService> _logger;

	public TelegramAuthorizationService(IServiceProvider serviceProvider)
	{
		_tdClient = serviceProvider.GetRequiredService<TdClient>();
		_logger = serviceProvider.GetRequiredService<ILogger<TelegramAuthorizationService>>();

		_logger.LogInformation("{@LogData}",
			new InitializeObjectLogDataWithTdClient
			{
				ContainerType = GetType().Name,
				Status = ExecutorLogDataStatus.Created,
				TdClientHash = _tdClient.GetHashCode().ToString(),
			});
	}

	public async Task SendCodeAsync(string code)
	{
		await _tdClient.ExecuteAsync(new CheckAuthenticationCode
		{
			Code = code
		});
	}

	public async Task SendPasswordAsync(string password)
	{
		await _tdClient.ExecuteAsync(new CheckAuthenticationPassword
		{ Password = password }
		);
	}
}
