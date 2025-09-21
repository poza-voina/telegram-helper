using static TdLib.TdApi;

namespace TelegramHelper.Core.Executors.Interfaces;

public interface IUpdateAuhorizationStateExecutor
{
    event EventHandler<AuthorizationState>? WaitEvent;
    Task ExecuteStateAsync(AuthorizationState state);
}
