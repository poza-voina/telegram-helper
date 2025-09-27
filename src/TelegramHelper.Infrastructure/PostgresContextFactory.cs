using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TelegramHelper.Infrastructure;

public class PostgresContextFactory : IDesignTimeDbContextFactory<PostgresContext>
{
    public PostgresContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=telegram;Username=postgres;Password=postgres"
        );

        return new PostgresContext(optionsBuilder.Options);
    }
}
