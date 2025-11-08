using TelegramHelper.Contracts.Folders;

namespace TelegramHelper.Core.Services.Interfaces;

public interface IFolderService
{
	Task<IEnumerable<FolderView>> GetCurrentFolders();
	Task FolderToArchiveAsync(long id);
	Task FolderFromArhive(long id);
}
