using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Configurations;

public class CurrentChatFolderModelConfiguration : IEntityTypeConfiguration<CurrentChatFolderModel>
{
    public void Configure(EntityTypeBuilder<CurrentChatFolderModel> builder)
    {
        builder
            .ToTable("chat_folder");

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
            .HasColumnName("status").HasConversion<string>();
    }
}
