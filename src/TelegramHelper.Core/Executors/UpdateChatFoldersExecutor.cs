using Microsoft.EntityFrameworkCore;
using TelegramHelper.Abstractions.Data;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.ObjectStorage.Mappers;
using TelegramHelper.Infrastructure.Repositories.Interfaces;
using static TdLib.TdApi;
using static TdLib.TdApi.Update;

namespace TelegramHelper.Core.Executors;

public class UpdateChatFoldersExecutor(
    ITelegramClient telegramClient,
    IFolderRepository folderRepository,
    IChatFolderRepository chatFolderRepository) : IUpdateChatFoldersExecutor
{
    public async Task ExecuteAsync(UpdateChatFolders @event)
    {
        var ownerId = 0;
        var folders = @event.ChatFolders.Select(x => x.TelegramFolderToCurrentFolderModel(ownerId));

        await folderRepository.UpdateOrCreateRangeAsync(folders);

        var chatsByFolders = await Task.WhenAll(
            folders.Select(
                async x =>
                {
                    return (await telegramClient.GetChatsByChatFolderAsync(x.FolderId)).ChatFolderToChatByFolder(x.FolderId);
                })
            );

        await chatFolderRepository
            .UpdateOrCreateRangeAsync(
                chatsByFolders.SelectMany(x => x.ChatsByFolderToCurrentChatFolderModel()));
    }
}
