using System.Security.Cryptography;
using TdLib;
using static TdLib.TdApi;
using static TdLib.TdApi.AuthorizationState;

namespace TelegramHelper.Core.Executors.Interfaces;

public interface IUpdateAuthorizationStateExecutor : IUpdateExecutor<TdApi.Update.UpdateAuthorizationState>
{
    event EventHandler<AuthorizationState>? WaitEvent;
    Task<(AuthorizationStateReady StateReady, long OwnerId)> WaitForReadyStateAsync(CancellationToken cancellationToken = default);
}
