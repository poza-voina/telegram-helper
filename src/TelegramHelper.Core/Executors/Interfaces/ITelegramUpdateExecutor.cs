using TdLib;

namespace TelegramHelper.Core.Executors.Interfaces;

public interface ITelegramUpdateExecutor : IUpdateExecutor<TdApi.Update>
{
    Task ExecuteAsync(TdApi.Update @event);
}