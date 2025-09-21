using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage;
using static TdLib.TdApi;
using static TdLib.TdApi.AuthorizationState;

namespace TelegramHelper.Core.Executors;

public class UpdateAuthorizationStateExecutor(
    InitializeClientOptions initializeClientOptions,
    TdLib.TdClient client) : IUpdateAuhorizationStateExecutor
{
    private readonly InitializeTelegramClientOptions _initializeOptions = initializeClientOptions.InitializeOptions;
    private readonly AuthorizeOptions _authorizeOptions = initializeClientOptions.AuthorizeOptions;
    public event EventHandler<AuthorizationState>? WaitEvent;

    public async Task ExecuteStateAsync(AuthorizationState state)
    {
        switch (state)
        {
            case AuthorizationStateWaitTdlibParameters:
                await ProcessTdLibParamentersAsync();
                break;
            case AuthorizationStateWaitPhoneNumber:
                await ProccessPhoneNumberAsync();
                break;
            case AuthorizationStateReady:
            case AuthorizationStateLoggingOut:
            case AuthorizationStateClosing:
            case AuthorizationStateClosed:
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

    private async Task ProcessClosed()
    {
        throw new NotImplementedException();
    }

    private async Task ProcessLoggingClosing()
    {
        throw new NotImplementedException();
    }

    private async Task ProcessLoggingOut()
    {
        throw new NotImplementedException();
    }

    private async Task ProcessReady()
    {
        throw new NotImplementedException();
    }

    private void GenerateEventState(AuthorizationState state)
    {
        Console.WriteLine($"🔥 Генерирую событие для стейта: {state}");
        WaitEvent?.Invoke(this, state);
    }

    private async Task ProccessPhoneNumberAsync()
    {
        var setNumber = await client.ExecuteAsync(
            new SetAuthenticationPhoneNumber { PhoneNumber = _authorizeOptions.PhoneNumber });
    }

    private async Task ProcessTdLibParamentersAsync()
    {
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

        await client.ExecuteAsync(param);
    }
}
