using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Repositories.Interfaces;

public interface IOwnerRepository : IRepository<OwnerModel>
{
    Task<OwnerModel> UpdateOrCreateAsync(OwnerModel model);
}
