using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Configurations;

public class CurrentDynamicFolderFilterModelConfiguration : IEntityTypeConfiguration<CurrentDynamicFolderFilterModel>
{
	public void Configure(EntityTypeBuilder<CurrentDynamicFolderFilterModel> builder)
	{
		builder
			.ToTable("current_dynamic_folder_filters");

		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.FolderId)
			.HasColumnName("folder_id");

		builder
			.Property(x => x.FilterType)
			.HasColumnName("filter_type")
			.HasConversion<string>();

		builder
			.HasOne(x => x.Folder)
			.WithMany(x => x.DynamicFilters)
			.HasForeignKey(x => x.FolderId);
	}
}

