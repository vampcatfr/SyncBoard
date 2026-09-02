using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SyncBoard.Domain.Columns;

namespace SyncBoard.Infrastructure.Persistence.Configurations;

public class ColumnConfiguration : IEntityTypeConfiguration<Column>
{
    public void Configure(EntityTypeBuilder<Column> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Position)
            .IsRequired();

        builder.HasMany(x => x.Cards)
            .WithOne(x => x.Column)
            .HasForeignKey(x => x.ColumnId);
    }
}