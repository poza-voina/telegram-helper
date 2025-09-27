using TdLib;
using static TdLib.TdApi.AuthorizationState;

namespace TelegramHelper.Core.Executors.Interfaces;

public interface ITelegramUpdateExecutor : IUpdateExecutor<TdApi.Update>
{
    ITelegramUpdateExecutor AddExecutor<T>() where T : IUpdateExecutor;
    T GetParentExecutor<T>() where T : IUpdateExecutor;
    Task<(AuthorizationStateReady StateReady, long OwnerId)> WaitForReadyStateAsync(CancellationToken cancellationToken = default);
}