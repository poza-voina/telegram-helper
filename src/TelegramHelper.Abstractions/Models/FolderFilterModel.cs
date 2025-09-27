using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Abstractions.Models;

public class FolderFilterModel : IDatabaseModel<long>
{
    public long Id { get; set; }
    public long FolderId { get; set; }
    public FolderFilterType FilterType { get; set; }
}