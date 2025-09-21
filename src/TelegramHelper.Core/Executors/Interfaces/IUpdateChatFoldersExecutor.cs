using static TdLib.TdApi.Update;

namespace TelegramHelper.Core.Executors.Interfaces;

public interface IUpdateChatFoldersExecutor : IUpdateExecutor<UpdateChatFolders>
{
    new Task ExecuteAsync(UpdateChatFolders @event);
}
