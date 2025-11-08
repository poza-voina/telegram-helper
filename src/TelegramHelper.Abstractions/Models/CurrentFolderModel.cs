using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Abstractions.Models;

public class CurrentFolderModel : IDatabaseModel<long>
{
	public long Id { get; set; }
	public required long OwnerId { get; set; }
	public int? TelegramFolderId { get; set; }
	public required string IconName { get; set; }
	public required string FolderName { get; set; }
	public DateTime CreateAt { get; set; }
	public DateTime UpdateAt { get; set; }
	public bool IsArchive { get; set; }
	public virtual ICollection<CurrentStaticFolderFilterModel>? StaticFilters { get; set; }
	public virtual ICollection<CurrentDynamicFolderFilterModel>? DynamicFilters { get; set; }
	public virtual OwnerModel? Owner { get; set; }
}
