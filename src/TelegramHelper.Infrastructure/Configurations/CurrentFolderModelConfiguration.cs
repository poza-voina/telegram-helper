using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramHelper.Abstractions.Models;
using TelegramHelper.Abstractions.Models.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TelegramHelper.Infrastructure.Configurations;

public class CurrentFolderModelConfiguration : IEntityTypeConfiguration<CurrentFolderModel>
{
    public void Configure(EntityTypeBuilder<CurrentFolderModel> builder)
    {
        builder
            .ToTable("folder");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .Property(x => x.OwnerId)
            .HasColumnName("owner_id");

        builder
            .Property(x => x.FolderId)
            .HasColumnName("folder_id");

        builder
            .HasIndex(x => new { x.OwnerId, x.FolderId })
            .IsUnique();

        builder
            .Property(x => x.IconName)
            .HasColumnName("icon_name");

        builder
            .Property(x => x.FolderName)
            .HasColumnName("folder_name");

        builder
            .Property(x => x.CreateAt)
            .HasColumnName("create_at")
            .HasDefaultValueSql("timezone('utc', now())");

        builder
            .Property(x => x.UpdateAt)
            .HasColumnName("update_at")
            .ValueGeneratedOnUpdate();
    }
}
