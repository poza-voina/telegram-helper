using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using TdLib;
using TelegramHelper.Abstractions;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.ObjectStorage.LogObjects;
using TelegramHelper.Core.Services.Interfaces;
using static TdLib.TdApi;

namespace TelegramHelper.Core.ObjectStorage;

public class TelegramClient : IDisposable, ITelegramClient
{
	public TdClient TdClient { get; protected init; }
	public ITelegramAuthorizationService AuthorizationService { get; protected set; }
	public IServiceProvider ScopedServiceProvider { get; protected init; }
	public IServiceScope Scope { get; protected init; }
	public ITelegramUpdateExecutor MainExecutor { get; protected init; }
	public TelegramClientContext Context { get; private init; }
	public Channel<Func<Task>> DbJobs { get; } = Channel.CreateUnbounded<Func<Task>>();
	private readonly ILogger<TelegramClient> _logger;

	protected TelegramClient(IServiceProvider serviceProvider, InitializeClientOptions initializeOptions)
	{
		Scope = serviceProvider.CreateScope();
		ScopedServiceProvider = Scope.ServiceProvider;

		Context = ScopedServiceProvider.GetRequiredService<TelegramClientContext>();
		Context.Status = TelegramClientStatus.Pending;
		Context.InitializeClientOptions = initializeOptions;

		MainExecutor = ScopedServiceProvider.GetRequiredService<ITelegramUpdateExecutor>();
		TdClient = ScopedServiceProvider.GetRequiredService<TdClient>();
		_logger = ScopedServiceProvider.GetRequiredService<ILogger<TelegramClient>>();

		MainExecutor
			.AddExecutor<IUpdateAuthorizationStateExecutor>();

		TdClient.UpdateReceived += async (sender, update) =>
		{
			await MainExecutor.ExecuteAsync(update);
		};

		AuthorizationService = ScopedServiceProvider.GetRequiredService<ITelegramAuthorizationService>();

		_logger.LogInformation("{@LogData}",
			new InitializeObjectLogDataWithTdClient
			{
				ContainerType = GetType().Name,
				TdClientHash = TdClient.GetHashCode().ToString(),
				Status = ExecutorLogDataStatus.Created
			});
	}

	public static TelegramClient InitializeFirstStep(
		IServiceProvider serviceProvider,
		InitializeClientOptions options)
	{
		//TODO видимо убрать
		var client = new TelegramClient(serviceProvider, options);

		return client;
	}

	public async Task<TelegramClient> InitializeSecondStep()
	{
		var tcs = new TaskCompletionSource<ITelegramClient>();

		MainExecutor
			.AddExecutor<IUpdateChatFoldersExecutor>();

		var (state, ownerId) = await MainExecutor.WaitForReadyStateAsync();

		Context.OwnerId = ownerId;

		Context.Status = TelegramClientStatus.Ready;

		_ = Task.Run(() => ProcessDbJobsAsync());

		_logger.LogInformation("{@LogData}",
			new InitializeObjectLogDataWithTdClient
			{
				ContainerType = GetType().Name,
				TdClientHash = TdClient.GetHashCode().ToString(),
				Status = ExecutorLogDataStatus.Initialized
			});

		return this;
	}

	public IServiceProvider ServiceProvider => ScopedServiceProvider;

	protected async Task ProcessDbJobsAsync(CancellationToken cancellationToken = default)
	{
		await foreach (var job in DbJobs.Reader.ReadAllAsync(cancellationToken))
		{
			try
			{
				_logger.LogInformation("Process job"); //TODO сделать
				await job();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при выполнении задачи: {ex}");
			}
		}
	}

	public void Dispose()
	{
		TdClient?.Dispose();
	}

	public async Task RemoveFolderAsync(int folderId)
	{
		TelegramClientException.ThrowIfStatusOnWorked(Context.Status);

		await TdClient.ExecuteAsync(new TdApi.DeleteChatFolder() { ChatFolderId = folderId });
	}

	public async Task<ChatFolder> GetChatsByChatFolderAsync(int folderId)
	{
		return await TdClient.ExecuteAsync(
			new GetChatFolder
			{
				ChatFolderId = folderId
			});
	}

	public async Task WakeUp()
	{
		await TdClient.ExecuteAsync(new TdApi.LoadChats
		{
			ChatList = new TdApi.ChatList.ChatListMain(),
			Limit = 10000
		});
	}

	public async Task<ChatFolderInfo> CreateFolderAsync(ChatFolder chatFolder)
	{
		return await TdClient.ExecuteAsync(new CreateChatFolder { Folder = chatFolder });
	}
}