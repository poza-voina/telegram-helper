using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Repositories.Interfaces;

public interface IChatFolderRepository : IRepository<CurrentChatFolderModel>
{
    Task<IEnumerable<CurrentChatFolderModel>> UpdateOrCreateRangeAsync(IEnumerable<CurrentChatFolderModel> models);
}
