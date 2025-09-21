using TelegramHelper.Abstractions.Data;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Abstractions.Models.Enums;
using static TdLib.TdApi;

namespace TelegramHelper.Core.ObjectStorage.Mappers;

public static class ChatByFolderMapper
{
    public static IEnumerable<CurrentChatFolderModel> ChatsByFolderToCurrentChatFolderModel(this ChatsByFolder src)
    {
        var folderId = src.FolderId;

        var exluded = src.ExcludedChatIds.Select(x => new CurrentChatFolderModel { ChatId = x, FolderId = folderId, Status = ChatStatus.Excluded });
        var included = src.IncludedChatIds.Select(x => new CurrentChatFolderModel { ChatId = x, FolderId = folderId, Status = ChatStatus.Included });

        return exluded.Concat(included);
    }

    public static ChatsByFolder ChatFolderToChatByFolder(this ChatFolder src, int folderId)
    {
        return new ChatsByFolder
        {
            FolderId = folderId,
            IncludedChatIds = src.IncludedChatIds,
            ExcludedChatIds = src.ExcludedChatIds
        };
    }
}
