using TelegramHelper.Abstractions.Data;
using static TdLib.TdApi;

namespace TelegramHelper.Core.ObjectStorage.Interfaces;

public interface ITelegramClient
{
    Task<ChatFolder> GetChatsByChatFolderAsync(int folderId);
    Task RemoveFolderAsync(int folderId);
    Task Test();
}
