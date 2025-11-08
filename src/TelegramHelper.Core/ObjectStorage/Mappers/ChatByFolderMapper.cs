using TelegramHelper.Abstractions.Data;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Abstractions.Models.Enums;
using static TdLib.TdApi;

namespace TelegramHelper.Core.ObjectStorage.Mappers;

public static class ChatByFolderMapper
{
	public static IEnumerable<CurrentStaticFolderFilterModel> FolderFiltersToCurrentStaticFolderFilterModel(this FolderFilters src)
	{
		var folderId = src.FolderId;

		var exluded = src.ExcludedChatIds.Select(x => new CurrentStaticFolderFilterModel { ChatId = x, FolderId = folderId, Status = ChatStatus.Excluded });
		var included = src.IncludedChatIds.Select(x => new CurrentStaticFolderFilterModel { ChatId = x, FolderId = folderId, Status = ChatStatus.Included });

		return exluded.Concat(included);
	}

	public static IEnumerable<CurrentDynamicFolderFilterModel> FolderFiltersToCurrentDynamicFilterModel(this FolderFilters src)
	{
		var folderId = src.FolderId;

		return src.DynemicFilters.Select(x => new CurrentDynamicFolderFilterModel
		{
			FolderId = folderId,
			FilterType = x
		});
	}

	public static FolderFilters ChatFolderToFolderFilters(this ChatFolder src, long folderId)
	{
		var mapping = new (Func<ChatFolder, bool> selector, FolderFilterType type)[]
		{
			(x => x.IncludeContacts, FolderFilterType.IncludeContacts),
			(x => x.IncludeNonContacts, FolderFilterType.IncludeNonContacts),
			(x => x.IncludeBots, FolderFilterType.IncludeBots),
			(x => x.IncludeGroups, FolderFilterType.IncludeGroups),
			(x => x.IncludeChannels, FolderFilterType.IncludeChannels),
		};

		var dynemicFilters = mapping.Where(x => x.selector(src))
			.Select(x => x.type)
			.ToList();

		return new FolderFilters
		{
			FolderId = folderId,
			IncludedChatIds = src.IncludedChatIds,
			ExcludedChatIds = src.ExcludedChatIds,
			DynemicFilters = dynemicFilters
		};
	}
}