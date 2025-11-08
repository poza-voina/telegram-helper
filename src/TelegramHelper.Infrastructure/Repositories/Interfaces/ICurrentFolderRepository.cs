using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Repositories.Interfaces;

public interface ICurrentFolderRepository : IRepository<CurrentFolderModel>
{
	Task<IEnumerable<CurrentFolderModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentFolderModel> models);
}