using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Abstractions.Models;

public class CurrentStaticFolderFilterModel : IDatabaseModel<long>
{
	public long Id { get; set; }
	public required long ChatId { get; set; }
	public long FolderId { get; set; }
	public required ChatStatus Status { get; set; }
	public virtual CurrentFolderModel? Folder { get; set; }
}
