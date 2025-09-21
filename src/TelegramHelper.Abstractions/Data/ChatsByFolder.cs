namespace TelegramHelper.Abstractions.Data;

public class ChatsByFolder
{
    public required int FolderId { get; set; }
    public long[] IncludedChatIds { get; set; } = [];
    public long[] ExcludedChatIds { get; set; } = [];
}
