using TelegramHelper.Abstractions;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.Services.Interfaces;

namespace TelegramHelper.Core.ObjectStorage;

public class TelegramClientDispatcher(IServiceProvider serviceProvider) : ITelegramClientDispatcher
{
	Dictionary<Guid, TelegramClient?> clients = new();

	public Guid CreateClient(InitializeClientOptions options)
	{
		var client = TelegramClient.InitializeFirstStep(serviceProvider, options);
		var id = client.Context.InitializeClientOptions?.Id ?? throw new NotFoundException("Не найден идентификатор клиента");
		clients.Add(id, client);

		_ = Task.Run(client.InitializeSecondStep);

		return id;
	}

	public ITelegramAuthorizationService GetAuthorizationService(Guid id)
	{
		return GetClientById(id).AuthorizationService;
	}

	public ITelegramClient GetTelegramClient(Guid id)
	{
		return GetClientById(id);
	}

	public ITelegramClient GetReadyTelegramClient(Guid id)
	{
		var client = GetClientById(id);

		if (client.Context.Status == TelegramClientStatus.Ready)
		{
			return client;
		}

		throw new InvalidOperationException($"Клиент с идентификатором {id} еще не готов");
	}

	public async Task<ITelegramClient> Wait(Guid id, CancellationToken cancellationToken = default)
	{
		await GetAuthorizationStateExecutor(id)
			.WaitForReadyStateAsync(cancellationToken);

		var client = GetClientById(id);

		await client.WakeUp();

		return client;
	}

	private TelegramClient GetClientById(Guid id)
	{
		if (clients.TryGetValue(id, out var client))
		{
			if (client is { })
			{
				return client;
			}

			throw new InvalidOperationException($"Клиент с id = {id} был удален");
		}

		throw new InvalidOperationException($"Клиент не найден с id = {id}");
	}

	public IUpdateAuthorizationStateExecutor GetAuthorizationStateExecutor(Guid id)
	{
		return GetClientById(id)
			.MainExecutor
			.GetParentExecutor<IUpdateAuthorizationStateExecutor>();
	}
}
