using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Abstractions.Models;

public class CurrentDynamicFolderFilterModel : IDatabaseModel<long>
{
	public long Id { get; set; }
	public long FolderId { get; set; }
	public FolderFilterType FilterType { get; set; }
	public virtual CurrentFolderModel? Folder { get; set; }
}