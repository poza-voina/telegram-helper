using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Configurations;

public class CurrentFolderModelConfiguration : IEntityTypeConfiguration<CurrentFolderModel>
{
	public void Configure(EntityTypeBuilder<CurrentFolderModel> builder)
	{
		builder
			.ToTable("current_folders");

		builder
			.HasKey(x => x.Id);

		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.OwnerId)
			.HasColumnName("owner_id");

		builder
			.Property(x => x.TelegramFolderId)
			.HasColumnName("telegram_folder_id");

		builder
			.Property(x => x.IsArchive)
			.HasColumnName("is_arhive")
			.IsRequired();

		builder
			.HasIndex(x => new { x.OwnerId, x.TelegramFolderId })
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
