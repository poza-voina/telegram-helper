using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Configurations;

public class FolderFilterConfiguration : IEntityTypeConfiguration<FolderFilterModel>
{
    public void Configure(EntityTypeBuilder<FolderFilterModel> builder)
    {
        builder
            .ToTable("folder_filters");

        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .Property(x => x.FolderId)
            .HasColumnName("folder_id");

        builder
            .Property(x => x.FilterType)
            .HasColumnName("filter_type");
    }
}
