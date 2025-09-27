using TelegramHelper.Abstractions.Data;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Abstractions.Models.Enums;
using TelegramHelper.Contracts.Folders;
using static TdLib.TdApi;

namespace TelegramHelper.Core.ObjectStorage.Mappers;

public static class FolderMapper
{
    public static CurrentFolderModel TelegramFolderToCurrentFolderModel(this ChatFolderInfo src, long ownerId)
    {
        return new CurrentFolderModel()
        {
            OwnerId = ownerId,
            FolderId = src.Id,
            FolderName = src.Name.Text.Text,
            IconName = src.Icon.Name,
        };
    }

    public static FolderView CurrentFolderModelToFolderView(this CurrentFolderModel src)
    {
        return new FolderView
        {
            Id = src.Id,
            OwnerId = src.OwnerId,
            FolderId = src.FolderId,
            IconName = src.IconName,
            FolderName = src.FolderName,
        };
    }
}
