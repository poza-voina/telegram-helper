using System.Threading.Channels;
using TdLib;
using TelegramHelper.Core.Services.Interfaces;
using static TdLib.TdApi;

namespace TelegramHelper.Core.ObjectStorage.Interfaces;

public interface ITelegramClient
{
	ITelegramAuthorizationService AuthorizationService { get; }
	IServiceProvider ServiceProvider { get; }
	TdClient TdClient { get; }
	TelegramClientContext Context { get; }
	Task<ChatFolder> GetChatsByChatFolderAsync(int folderId);
	Task RemoveFolderAsync(int folderId);
	Channel<Func<Task>> DbJobs { get; }
	Task WakeUp();
	Task<ChatFolderInfo> CreateFolderAsync(ChatFolder chatFolder);
}
