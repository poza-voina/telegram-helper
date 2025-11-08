using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Repositories.Interfaces;

public interface ICurrentStaticFolderFilterRepository : IRepository<CurrentStaticFolderFilterModel>
{
    Task<IEnumerable<CurrentStaticFolderFilterModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentStaticFolderFilterModel> models);
}