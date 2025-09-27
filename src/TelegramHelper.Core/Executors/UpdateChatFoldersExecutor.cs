using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.ObjectStorage.LogObjects;
using TelegramHelper.Core.ObjectStorage.Mappers;
using TelegramHelper.Infrastructure.Repositories.Interfaces;
using static TdLib.TdApi.Update;

namespace TelegramHelper.Core.Executors;

public class UpdateChatFoldersExecutor : BaseUpdateExecutor<UpdateChatFolders>, IUpdateChatFoldersExecutor
{
    private readonly ITelegramClient _telegramClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly TelegramClientContext _telegramClientContext;
    private readonly ILogger<UpdateChatFoldersExecutor> _logger;

    public UpdateChatFoldersExecutor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var dispatcher = _serviceProvider.GetRequiredService<ITelegramClientDispatcher>();
        _telegramClientContext = _serviceProvider.GetRequiredService<TelegramClientContext>();
        _logger = _serviceProvider.GetRequiredService<ILogger<UpdateChatFoldersExecutor>>();

        var id = _telegramClientContext.InitializeClientOptions?.Id;
        NotFoundException.ThrowIfNull(id);
        _telegramClient = dispatcher.GetTelegramClient(id.Value);


        _logger.LogInformation("{@LogData}",
            new InitializeObjectLogData
            {
                ContainerType = GetType().Name,
                Status = ExecutorLogDataStatus.Created,
            });
    }

    public override async Task ExecuteAsync(UpdateChatFolders @event)
    {
        _logger.LogInformation("{@LogData}",
            new EventRecivedLogData
            {
                ContainerType = GetType().Name,
                EventType = @event.DataType
            });
        await _telegramClient.DbJobs.Writer.WriteAsync(() => ExecuteBodyAsync(@event));
    }

    private async Task ExecuteBodyAsync(UpdateChatFolders @event)
    {
        NotFoundException.ThrowIfNull(_telegramClientContext.OwnerId);

        using var scope = _serviceProvider.CreateScope();
        var _folderRepository = scope.ServiceProvider.GetRequiredService<IFolderRepository>();
        var _chatFolderRepository = scope.ServiceProvider.GetRequiredService<IChatFolderRepository>();

        var folders = @event
            .ChatFolders
            .Select(x => x.TelegramFolderToCurrentFolderModel(_telegramClientContext.OwnerId!.Value));

        await _folderRepository.UpdateOrCreateRangeAsync(folders);

        var chatsByFolders = await Task.WhenAll(
            folders.Select(
                async x =>
                {
                    return (await _telegramClient.GetChatsByChatFolderAsync(x.FolderId)).ChatFolderToChatByFolder(x.FolderId);
                })
            );

        await _chatFolderRepository
            .UpdateOrCreateRangeAsync(
                chatsByFolders.SelectMany(x => x.ChatsByFolderToCurrentChatFolderModel()));
    }
}
