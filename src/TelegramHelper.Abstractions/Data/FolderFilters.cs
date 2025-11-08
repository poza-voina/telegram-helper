using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Abstractions.Data;

public class FolderFilters
{
	public required long FolderId { get; set; }
	public long[] IncludedChatIds { get; set; } = [];
	public long[] ExcludedChatIds { get; set; } = [];
	public IEnumerable<FolderFilterType> DynemicFilters = [];
}
