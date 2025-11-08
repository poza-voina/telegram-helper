using Microsoft.EntityFrameworkCore;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Infrastructure.Repositories.Interfaces;

namespace TelegramHelper.Infrastructure.Repositories;

public class CurrentStaticFolderFilterRepository(PostgresContext context)
	: Repository<CurrentStaticFolderFilterModel>(context), ICurrentStaticFolderFilterRepository
{
	public async Task<IEnumerable<CurrentStaticFolderFilterModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentStaticFolderFilterModel> models)
	{
		var modelsList = models.ToList();

		var chatIdKeys = modelsList.Select(x => x.ChatId)
			.ToHashSet();
		var folderIdKeys = modelsList.Select(x => x.FolderId)
			.ToHashSet();

		var exists = await context.CurrentStaticFolderFilters
			.Where(
				x => chatIdKeys.Contains(x.ChatId) &&
				folderIdKeys.Contains(x.FolderId))
			.ToListAsync();

		var toAdd = new List<CurrentStaticFolderFilterModel>();

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