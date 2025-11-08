using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Configurations;

public class CurrentStaticFolderFilterModelConfiguration : IEntityTypeConfiguration<CurrentStaticFolderFilterModel>
{
	public void Configure(EntityTypeBuilder<CurrentStaticFolderFilterModel> builder)
	{
		builder
			.ToTable("current_static_folder_filters");

		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.ChatId)
			.HasColumnName("chat_id");

		builder
			.Property(x => x.FolderId)
			.HasColumnName("folder_id");

		builder
			.Property(x => x.Status)
			.HasColumnName("status")
			.HasConversion<string>();

		builder
			.HasOne(x => x.Folder)
			.WithMany(x => x.StaticFilters)
			.HasForeignKey(x => x.FolderId);
	}
}
