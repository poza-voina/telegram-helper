using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Cryptography.X509Certificates;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Infrastructure.Configurations;

public class OwnerModelConfiguration : IEntityTypeConfiguration<OwnerModel>
{
    public void Configure(EntityTypeBuilder<OwnerModel> builder)
    {
        builder
            .ToTable("owner");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .Property(x => x.PhoneNumber)
            .HasColumnName("phone_number");

        builder
            .Property(x => x.FirstName)
            .HasColumnName("first_name");

        builder
            .Property(x => x.LastName)
            .HasColumnName("last_name");

        builder
            .HasMany(x => x.CurrentFolders)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId);
    }
}
