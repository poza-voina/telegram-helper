using Microsoft.EntityFrameworkCore;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Abstractions.Models.Enums;
using TelegramHelper.Contracts.Folders;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.ObjectStorage.Mappers;
using TelegramHelper.Core.Services.Interfaces;
using TelegramHelper.Infrastructure.Repositories.Interfaces;

namespace TelegramHelper.Core.Services;

public class FolderService(ITelegramClient telegramClient, IRepository<CurrentFolderModel> folderRepository) : IFolderService
{
    public async Task FolderToArchive(long id)
    {
        var model = await folderRepository.FindOrDefaultAsync(id);

        NotFoundException.ThrowIfNull(model);

        await telegramClient.RemoveFolderAsync(model.FolderId);

        await folderRepository.UpdateAsync(model);
    }

    public async Task<IEnumerable<FolderView>> GetCurrentFolders(long ownerId)
    {
        var query = folderRepository
            .GetAll()
            .Where(x => x.OwnerId == ownerId)
            .Select(x => x.CurrentFolderModelToFolderView());

        return await query.ToListAsync();
    }
}
