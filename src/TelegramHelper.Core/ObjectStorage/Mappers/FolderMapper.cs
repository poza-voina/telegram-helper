using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Abstractions.Models.Enums;
using TelegramHelper.Contracts.Folders;
using TelegramHelper.Core.Extensions;
using static TdLib.TdApi;

namespace TelegramHelper.Core.ObjectStorage.Mappers;

public static class FolderMapper
{
	public static CurrentFolderModel TelegramFolder_To_CurrentFolderModel(this ChatFolderInfo src, long ownerId)
	{
		return new CurrentFolderModel()
		{
			OwnerId = ownerId,
			TelegramFolderId = src.Id,
			FolderName = src.Name.Text.Text,
			IconName = src.Icon.Name,
		};
	}

	public static ChatFolder CurrentFolderModel_To_TelegramFolder(this CurrentFolderModel src)
	{
		NotFoundException.ThrowIfNull(src.StaticFilters);
		NotFoundException.ThrowIfNull(src.DynamicFilters);

		return new ChatFolder()
			.WithName(src.FolderName)
			.WithIcon(src.IconName)
			.WithDynamicFilters(src.DynamicFilters)
			.WithStaticFilters(src.StaticFilters);
	}

	public static FolderView CurrentFolderModel_To_FolderView(this CurrentFolderModel src)
	{
		return new FolderView
		{
			Id = src.Id,
			OwnerId = src.OwnerId,
			FolderId = src.TelegramFolderId,
			IconName = src.IconName,
			FolderName = src.FolderName,
		};
	}
}
