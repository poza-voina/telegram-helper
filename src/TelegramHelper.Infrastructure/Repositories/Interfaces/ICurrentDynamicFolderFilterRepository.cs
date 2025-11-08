using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Repositories.Interfaces;

public interface ICurrentDynamicFolderFilterRepository : IRepository<CurrentDynamicFolderFilterModel>
{
    Task<IEnumerable<CurrentDynamicFolderFilterModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentDynamicFolderFilterModel> models);
}
