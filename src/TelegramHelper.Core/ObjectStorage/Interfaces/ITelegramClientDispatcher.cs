using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.Services.Interfaces;

namespace TelegramHelper.Core.ObjectStorage.Interfaces;

public interface ITelegramClientDispatcher
{
	Guid CreateClient(InitializeClientOptions options);
	ITelegramClient GetTelegramClient(Guid id);
	ITelegramClient GetReadyTelegramClient(Guid id);
	Task<ITelegramClient> Wait(Guid id, CancellationToken cancellationToken = default);
	IUpdateAuthorizationStateExecutor GetAuthorizationStateExecutor(Guid id);
	ITelegramAuthorizationService GetAuthorizationService(Guid id);
}
