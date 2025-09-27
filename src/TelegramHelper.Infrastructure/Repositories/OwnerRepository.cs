using Microsoft.EntityFrameworkCore;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Infrastructure.Repositories.Interfaces;

namespace TelegramHelper.Infrastructure.Repositories;

public class OwnerRepository(PostgresContext context) : Repository<OwnerModel>(context), IOwnerRepository
{
    public async Task<OwnerModel> UpdateOrCreateAsync(OwnerModel model)
    {
        var exist = await context.Owners.FirstOrDefaultAsync(x => x.Id == model.Id);

        if (exist is { })
        {
            UpdateEntryWithoutPK(model, exist);
            await context.SaveChangesAsync();
            return exist;
        }
        else
        {
            await context.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }
    }
}
