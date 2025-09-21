using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Abstractions.Models;

public class CurrentFolderModel : IDatabaseModel<long>
{
    public long Id { get; set; }
    public string? Description { get; set; }
    public required long OwnerId { get; set; }
    public required int FolderId { get; set; }
    public required string IconName { get; set; }
    public required string FolderName { get; set; }
    public required FolderType FolderType { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public virtual IEnumerable<CurrentChatFolderModel> ChatModels { get; set; } = [];
}
