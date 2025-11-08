using TelegramHelper.Core.Executors.Interfaces;

namespace TelegramHelper.Core.Executors;

public abstract class BaseUpdateExecutor<T> : IUpdateExecutor<T> where T : TdLib.TdApi.Update
{
	public Type UpdateType => typeof(T);

	public abstract Task ExecuteAsync(T @event);

	public Task ExecuteAsync(object @event)
	{
		if (@event is T typedEvent)
		{
			ExecuteAsync(typedEvent);
		}

		return Task.CompletedTask;
	}
}