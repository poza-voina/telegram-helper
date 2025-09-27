using TdLib;

namespace TelegramHelper.Core.Executors.Interfaces;

public interface IUpdateExecutor
{
    Type UpdateType { get; }
    Task ExecuteAsync(object @event);
}

public interface IUpdateExecutor<in T> : IUpdateExecutor where T : TdApi.Update 
{
    Task ExecuteAsync(T @event);
}