using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Infrastructure.Repositories.Interfaces;

namespace TelegramHelper.Infrastructure.Repositories;

public class ChatFolderRepository(PostgresContext context) : Repository<CurrentChatFolderModel>(context), IChatFolderRepository
{
    public async Task<IEnumerable<CurrentChatFolderModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentChatFolderModel> models)
    {
        var modelsList = models.ToList();

        var chatIdKeys = modelsList.Select(x => x.ChatId)
            .ToHashSet();
        var folderIdKeys = modelsList.Select(x => x.FolderId)
            .ToHashSet();

        var exists = await context.ChatFolders
            .Where(
                x => chatIdKeys.Contains(x.ChatId) &&
                folderIdKeys.Contains(x.FolderId))
            .ToListAsync();

        var toAdd = new List<CurrentChatFolderModel>();

        foreach (var sourceModel in modelsList)
        {
            var destinationModel = exists.FirstOrDefault(x => x.ChatId == sourceModel.ChatId && x.FolderId == sourceModel.FolderId);

            if (destinationModel is null)
            {
                toAdd.Add(sourceModel);
            }
            else
            {
                UpdateEntryWithoutPK(sourceModel, destinationModel);
            }
        }

        await context.AddRangeAsync(toAdd);

        await context.SaveChangesAsync();

        return exists.Concat(toAdd);
    }
}

public class FolderRepository(PostgresContext context) : Repository<CurrentFolderModel>(context), IFolderRepository
{
    public async Task<IEnumerable<CurrentFolderModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentFolderModel> models)
    {
        var modelsList = models.ToList();

        var folderIdKeys = modelsList.Select(x => x.FolderId).ToHashSet();

        var exists = await context.Folders.Where(x => folderIdKeys.Contains(x.FolderId)).ToListAsync();

        var toAdd = new List<CurrentFolderModel>();

        foreach (var sourceModel in modelsList)
        {
            var desinationModel = exists.FirstOrDefault(x => x.FolderId == sourceModel.FolderId);
            if (desinationModel is null)
            {
                toAdd.Add(sourceModel);
            }
            else
            {
                UpdateEntryWithoutPK(sourceModel, desinationModel);
            }
        }

        await context.AddRangeAsync(toAdd);
        await context.SaveChangesAsync();

        return exists.Concat(toAdd);
    }
}
