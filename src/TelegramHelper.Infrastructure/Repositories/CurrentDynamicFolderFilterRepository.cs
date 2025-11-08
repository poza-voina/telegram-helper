using Microsoft.EntityFrameworkCore;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Infrastructure.Repositories.Interfaces;

namespace TelegramHelper.Infrastructure.Repositories;

public class CurrentDynamicFolderFilterRepository(PostgresContext context) : Repository<CurrentDynamicFolderFilterModel>(context), ICurrentDynamicFolderFilterRepository
{
	public async Task<IEnumerable<CurrentDynamicFolderFilterModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentDynamicFolderFilterModel> models)
	{
		{
			var modelsList = models.ToList();

			var folderIdKeys = modelsList.Select(x => x.FolderId)
				.ToHashSet();
			var typeKeys = modelsList.Select(x => x.FilterType)
				.ToHashSet();

			var exists = await context.CurrentDynamicFolderFilters
				.Where(
					x => folderIdKeys.Contains(x.FolderId) &&
					typeKeys.Contains(x.FilterType))
				.ToListAsync();

			var toAdd = new List<CurrentDynamicFolderFilterModel>();

			foreach (var sourceModel in modelsList)
			{
				var destinationModel = exists.FirstOrDefault(x => x.FilterType == sourceModel.FilterType && x.FolderId == sourceModel.FolderId);

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
}
