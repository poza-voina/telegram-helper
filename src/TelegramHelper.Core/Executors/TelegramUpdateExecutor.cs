using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TdLib;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage.LogObjects;
using static TdLib.TdApi.AuthorizationState;
using static TdLib.TdApi.Update;

namespace TelegramHelper.Core.Executors;

public class TelegramUpdateExecutor(IServiceProvider serviceProvider) : BaseUpdateExecutor<TdApi.Update>, ITelegramUpdateExecutor
{
	private List<IUpdateExecutor> _executors = new List<IUpdateExecutor>();
	private readonly IServiceProvider _serviceProvider = serviceProvider;
	private readonly ILogger<TelegramUpdateExecutor> logger = serviceProvider.GetRequiredService<ILogger<TelegramUpdateExecutor>>();

	public ITelegramUpdateExecutor AddExecutor<T>() where T : IUpdateExecutor
	{
		_executors.Add(_serviceProvider.GetRequiredService<T>());

		logger.LogInformation("{@LogData}",
			new ExecutorAddedLogData
			{
				ContainerType = GetType().Name,
				ExecutorType = typeof(T).Name
			});

		return this;
	}

	public override async Task ExecuteAsync(TdApi.Update @event)
	{
		logger.LogInformation("{@LogData}",
			new EventRecivedLogData
			{
				ContainerType = GetType().Name,
				EventType = @event.DataType,
			});

		var executor = _executors
			.FirstOrDefault(x => x.UpdateType.IsAssignableFrom(@event.GetType()));

		if (executor is not null)
		{
			await executor.ExecuteAsync(@event);
		}
	}

	public Task<(AuthorizationStateReady StateReady, long OwnerId)> WaitForReadyStateAsync(CancellationToken cancellationToken = default)
	{
		var executor = _executors.FirstOrDefault(x => typeof(UpdateAuthorizationState) == x.UpdateType) ?? throw new NotFoundException("Не найден executor авторизации");

		if (executor is IUpdateAuthorizationStateExecutor typedExector)
		{
			return typedExector.WaitForReadyStateAsync(cancellationToken);
		}

		throw new NotFoundException("Не найден executor авторизации");
	}

	public T GetParentExecutor<T>() where T : IUpdateExecutor
	{
		return _executors.OfType<T>().FirstOrDefault() ?? throw new InvalidOperationException("Экзекютор не найден");
	}
}
