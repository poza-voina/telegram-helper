using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
    public DbSet<CurrentFolderModel> Folders => Set<CurrentFolderModel>();
    public DbSet<CurrentChatFolderModel> ChatFolders => Set<CurrentChatFolderModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}