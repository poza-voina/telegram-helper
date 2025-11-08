using Microsoft.EntityFrameworkCore;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Infrastructure.Repositories.Interfaces;

namespace TelegramHelper.Infrastructure.Repositories;

public class CurrentFolderRepository(PostgresContext context) : Repository<CurrentFolderModel>(context), ICurrentFolderRepository
{
	public async Task<IEnumerable<CurrentFolderModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentFolderModel> models)
	{
		var modelsList = models.ToList();

		var folderIdKeys = modelsList.Select(x => x.TelegramFolderId).ToHashSet();

		var exists = await context.CurrentFolders.Where(x => folderIdKeys.Contains(x.TelegramFolderId)).ToListAsync();

		var toAdd = new List<CurrentFolderModel>();


		foreach (var sourceModel in modelsList)
		{
			var desinationModel = exists.FirstOrDefault(x => x.TelegramFolderId == sourceModel.TelegramFolderId);
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
