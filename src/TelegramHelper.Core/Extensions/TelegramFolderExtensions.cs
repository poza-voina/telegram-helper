using TelegramHelper.Abstractions.Models;
using TelegramHelper.Abstractions.Models.Enums;
using static TdLib.TdApi;

namespace TelegramHelper.Core.Extensions;

public static class TelegramFolderExtensions
{
	public static ChatFolder WithStaticFilters(this ChatFolder telegramFolder, ICollection<CurrentStaticFolderFilterModel> staticFilters)
	{
		var includedChatIds = staticFilters
			.Where(x => x.Status is ChatStatus.Included)
			.Select(x => x.ChatId)
			.ToArray();

		var exludedChatIds = staticFilters
			.Where(x => x.Status is ChatStatus.Excluded)
			.Select(x => x.ChatId)
			.ToArray();

		telegramFolder.IncludedChatIds = includedChatIds;
		telegramFolder.ExcludedChatIds = exludedChatIds;

		return telegramFolder;
	}

	public static ChatFolder WithName(this ChatFolder telegramFolder, string name)
	{
		telegramFolder.Name = new ChatFolderName
		{
			Text = new FormattedText
			{
				Text = name
			}
		};


		return telegramFolder;
	}

	public static ChatFolder WithIcon(this ChatFolder telegramFolder, string iconName)
	{
		telegramFolder.Icon = new ChatFolderIcon
		{
			Name = iconName
		};

		return telegramFolder;
	}

	public static ChatFolder WithDynamicFilters(this ChatFolder telegramFolder, ICollection<CurrentDynamicFolderFilterModel> filters)
	{
		var filtersTypes = filters.Select(x => x.FilterType).ToHashSet();

		telegramFolder.ExcludeMuted = filtersTypes.Contains(FolderFilterType.ExcludeMuted);
		telegramFolder.ExcludeRead = filtersTypes.Contains(FolderFilterType.ExcludeRead);
		telegramFolder.ExcludeArchived = filtersTypes.Contains(FolderFilterType.ExcludeArchived);
		telegramFolder.IncludeContacts = filtersTypes.Contains(FolderFilterType.IncludeContacts);
		telegramFolder.IncludeNonContacts = filtersTypes.Contains(FolderFilterType.IncludeNonContacts);
		telegramFolder.IncludeBots = filtersTypes.Contains(FolderFilterType.IncludeBots);
		telegramFolder.IncludeGroups = filtersTypes.Contains(FolderFilterType.IncludeGroups);
		telegramFolder.IncludeChannels = filtersTypes.Contains(FolderFilterType.IncludeChannels);

		return telegramFolder;
	}
}
