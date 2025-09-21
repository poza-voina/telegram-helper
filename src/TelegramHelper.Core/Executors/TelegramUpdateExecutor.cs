using TdLib;
using TelegramHelper.Core.Executors.Interfaces;
using static TdLib.TdApi.Update;

namespace TelegramHelper.Core.Executors;

public class TelegramUpdateExecutor(
    IUpdateAuhorizationStateExecutor updateAuhorizationStateExecutor,
    IUpdateChatFoldersExecutor updateChatFoldersExecutor) : ITelegramUpdateExecutor
{
    public async Task ExecuteAsync(TdApi.Update @event)
    {
        Console.WriteLine($"event = {@event.GetType()}");
        if (@event is UpdateAuthorizationState)
        {
            await updateAuhorizationStateExecutor.ExecuteStateAsync(((UpdateAuthorizationState)@event).AuthorizationState);
        }
        else if (@event is UpdateChatFolders)
        {
            await updateChatFoldersExecutor.ExecuteAsync((UpdateChatFolders)@event);
        }
    }
}
