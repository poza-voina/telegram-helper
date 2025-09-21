using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage.Interfaces;

namespace TelegramHelper.Core.ObjectStorage;

public class ClientServiceFactory(TdLib.TdClient TdClient, ITelegramUpdateExecutor updateExecutor)
{
    public async Task<ITelegramClient> CreateAsync(InitializeClientOptions initializeOptions)
    {
        return await TelegramClient.InitializeClient(TdClient, initializeOptions, updateExecutor);
    }
}
