using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Contracts.Folders;

public class FolderView
{
    public long Id { get; set; }
    public string? Description { get; set; }
    public required long OwnerId { get; set; }
    public required int FolderId { get; set; }
    public required string IconName { get; set; }
    public required string FolderName { get; set; }
    public required FolderType FolderType { get; set; }
}