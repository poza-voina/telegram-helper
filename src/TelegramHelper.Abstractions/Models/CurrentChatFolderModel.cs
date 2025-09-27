using TelegramHelper.Abstractions.Models.Enums;

namespace TelegramHelper.Abstractions.Models;

public class CurrentChatFolderModel : IDatabaseModel<long>
{
    public long Id { get; set; }
    public required long ChatId { get; set; }
    public required int FolderId { get; set; }
    public required ChatStatus Status { get; set; }
}
