namespace TelegramHelper.Abstractions.Models.Enums;

public enum FolderFilterType
{
	IncludeContacts,
	IncludeNonContacts,
	IncludeBots,
	IncludeGroups,
	IncludeChannels,
	ExcludeMuted,
	ExcludeRead,
	ExcludeArchived
}