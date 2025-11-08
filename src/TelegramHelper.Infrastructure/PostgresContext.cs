using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure;

public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
{
	public DbSet<CurrentFolderModel> CurrentFolders => Set<CurrentFolderModel>();
	public DbSet<CurrentStaticFolderFilterModel> CurrentStaticFolderFilters => Set<CurrentStaticFolderFilterModel>();
	public DbSet<CurrentDynamicFolderFilterModel> CurrentDynamicFolderFilters => Set<CurrentDynamicFolderFilterModel>();
	public DbSet<OwnerModel> Owners => Set<OwnerModel>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}