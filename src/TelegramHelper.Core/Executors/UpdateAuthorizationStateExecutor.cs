using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TdLib;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage;
using TelegramHelper.Core.ObjectStorage.LogObjects;
using TelegramHelper.Core.ObjectStorage.Mappers;
using TelegramHelper.Infrastructure.Repositories.Interfaces;
using static TdLib.TdApi;
using static TdLib.TdApi.AuthorizationState;
using static TdLib.TdApi.Update;

namespace TelegramHelper.Core.Executors;

public class UpdateAuthorizationStateExecutor
	: BaseUpdateExecutor<UpdateAuthorizationState>, IUpdateAuthorizationStateExecutor
{
	private readonly TdClient _tdClient;
	private readonly IServiceProvider _serviceProvider;
	private readonly InitializeTelegramClientOptions _initializeOptions;
	private readonly AuthorizeOptions _authorizeOptions;
	private readonly ILogger<UpdateAuthorizationStateExecutor> _logger;
	private readonly TaskCompletionSource<(AuthorizationStateReady StateReady, long OwnerId)> readyStateCompletionSource;
	public event EventHandler<AuthorizationState>? WaitEvent;

	public UpdateAuthorizationStateExecutor(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;

		_logger = _serviceProvider.GetRequiredService<ILogger<UpdateAuthorizationStateExecutor>>();

		var context = _serviceProvider.GetRequiredService<TelegramClientContext>();
		_tdClient = _serviceProvider.GetRequiredService<TdClient>();

		var initOptions = context.InitializeClientOptions;
		NotFoundException.ThrowIfNull(initOptions);

		_initializeOptions = initOptions.InitializeOptions;
		_authorizeOptions = initOptions.AuthorizeOptions;

		readyStateCompletionSource = new();

		_logger.LogInformation("{@LogData}",
			new InitializeObjectLogDataWithTdClient
			{
				ContainerType = GetType().Name,
				TdClientHash = _tdClient.GetHashCode().ToString(),
				Status = ExecutorLogDataStatus.Created
			});
	}

	public override async Task ExecuteAsync(UpdateAuthorizationState @event)
	{
		_logger.LogInformation("{@LogData}",
			new EventRecivedLogData
			{
				ContainerType = GetType().Name,
				EventType = @event.DataType,
				State = @event.AuthorizationState.DataType
			}
			);
		using var scope = _serviceProvider.CreateScope();
		var state = @event.AuthorizationState;

		switch (state)
		{
			case AuthorizationStateWaitTdlibParameters:
				await ProcessTdLibParamentersAsync();
				break;
			case AuthorizationStateWaitPhoneNumber:
				await ProccessPhoneNumberAsync();
				break;
			case AuthorizationStateReady:
				var ownerId = await GetUserInfo(scope);
				readyStateCompletionSource.TrySetResult(((AuthorizationStateReady)state, ownerId));
				break;
			case AuthorizationStateClosing:
			case AuthorizationStateClosed:
				readyStateCompletionSource.TrySetException(new InvalidOperationException("Авторизация по каким-то причинам завершилась"));
				break;
			case AuthorizationStateLoggingOut:
				break;
			case AuthorizationStateWaitPassword:
			case AuthorizationStateWaitRegistration:
			case AuthorizationStateWaitOtherDeviceConfirmation:
			case AuthorizationStateWaitEmailCode:
			case AuthorizationStateWaitEmailAddress:
			case AuthorizationStateWaitCode:
				GenerateEventState(state);
				break;
			default:
				throw new Exception("Нет обработчика стейта авторизации"); //TODO: сделать
		}
	}

	private async Task<long> GetUserInfo(IServiceScope scope)
	{
		_logger.LogInformation("{@LogData}",
			new ExecutorMethodLogData
			{
				ContainerType = GetType().Name,
				MethodName = nameof(GetUserInfo)
			});

		var response = await _tdClient.GetMeAsync();

		await scope.ServiceProvider.GetRequiredService<IOwnerRepository>().UpdateOrCreateAsync(response.TelegramUserToOwnerModel());

		return response.Id;
	}

	private void GenerateEventState(AuthorizationState state)
	{
		Console.WriteLine($"Генерирую событие для стейта: {state}");
		WaitEvent?.Invoke(this, state);
	}

	private async Task ProccessPhoneNumberAsync()
	{
		_logger.LogInformation("{@LogData}",
			new ExecutorMethodLogData
			{
				ContainerType = GetType().Name,
				MethodName = nameof(ProccessPhoneNumberAsync)
			});

		var setNumber = await _tdClient.ExecuteAsync(
			new SetAuthenticationPhoneNumber { PhoneNumber = _authorizeOptions.PhoneNumber });
	}

	private async Task ProcessTdLibParamentersAsync()
	{
		_logger.LogInformation("{@LogData}",
			new ExecutorMethodLogData
			{
				ContainerType = GetType().Name,
				MethodName = nameof(ProcessTdLibParamentersAsync)
			});
		var param = new SetTdlibParameters
		{
			UseTestDc = false,
			DatabaseDirectory = _initializeOptions.DatabaseDirectory,
			FilesDirectory = _initializeOptions.FilesDirectory,
			UseFileDatabase = true,
			UseChatInfoDatabase = true,
			UseMessageDatabase = true,
			UseSecretChats = false,
			ApiId = _initializeOptions.ApiId,
			ApiHash = _initializeOptions.ApiHash,
			SystemLanguageCode = "en",
			DeviceModel = "PC",
			ApplicationVersion = "1.0",
			SystemVersion = "Windows 10"
		};

		var result = await _tdClient.ExecuteAsync(param);
	}

	public async Task<(AuthorizationStateReady StateReady, long OwnerId)> WaitForReadyStateAsync(CancellationToken cancellationToken = default)
	{
		return await readyStateCompletionSource.Task.WaitAsync(cancellationToken);
	}
}
