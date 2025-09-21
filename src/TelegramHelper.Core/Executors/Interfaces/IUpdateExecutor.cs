using TdLib;

namespace TelegramHelper.Core.Executors.Interfaces;

public interface IUpdateExecutor<T> where T : TdApi.Update
{
    Task ExecuteAsync(T @event);
}