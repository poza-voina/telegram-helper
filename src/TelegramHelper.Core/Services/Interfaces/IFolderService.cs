using TelegramHelper.Contracts.Folders;

namespace TelegramHelper.Core.Services.Interfaces;

public interface IFolderService
{
    Task<IEnumerable<FolderView>> GetCurrentFolders(long ownerId);
    Task FolderToArchive(long id);
}
