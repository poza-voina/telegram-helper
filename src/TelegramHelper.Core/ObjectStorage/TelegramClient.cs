using TdLib;
using TelegramHelper.Abstractions;
using TelegramHelper.Abstractions.Data;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Core.Executors;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.ObjectStorage.Mappers;
using TelegramHelper.Infrastructure.Repositories.Interfaces;
using static TdLib.TdApi;
using static TdLib.TdApi.AuthorizationState;
using static TdLib.TdApi.Update;

namespace TelegramHelper.Core.ObjectStorage;

public class TelegramClient(TdClient client) : IDisposable, ITelegramClient
{
    public TdClient Client { get; } = client;
    public TelegramClientStatus Status { get; private set; }

    public static Task<ITelegramClient> InitializeClient(TdClient tdClient, InitializeClientOptions options, ITelegramUpdateExecutor updateExecutor)
    {
        var service = new TelegramClient(tdClient);
        var client = service.Client;
        var tcs = new TaskCompletionSource<ITelegramClient>();

        client.UpdateReceived += async (sender, update) =>
        {
            try
            {
                await updateExecutor.ExecuteAsync(update);

                if (update is UpdateAuthorizationState authUpdate &&
                    authUpdate.AuthorizationState is AuthorizationStateReady)

                {
                    tcs.TrySetResult(service);
                }
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        };

        return tcs.Task;
    }

    public void Dispose()
    {
        Client?.Dispose();
    }

    public async Task RemoveFolderAsync(int folderId)
    {
        TelegramClientException.ThrowIfStatusOnWorked(Status);

        await Client.ExecuteAsync(new TdApi.DeleteChatFolder() { ChatFolderId = folderId });
    }

    public async Task<ChatFolder> GetChatsByChatFolderAsync(int folderId)
    {
        return await Client.ExecuteAsync(
            new GetChatFolder
            {
                ChatFolderId = folderId
            });
    }

    public async Task Test()
    {
        await client.ExecuteAsync(new TdApi.LoadChats
        {
            ChatList = new TdApi.ChatList.ChatListMain(),
            Limit = 10000
        });
    }
}