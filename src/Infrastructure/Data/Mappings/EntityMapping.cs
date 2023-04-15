using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Common.Entities;

namespace Infrastructure.Data.Mappings;

public abstract class EntityMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.CreatedOn)
            .IsRequired()
            .HasColumnName("CreatedOn")
            .HasColumnType("DATETIME");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasColumnName("CreatedBy")
            .HasMaxLength(255)
            .HasColumnType("VARCHAR(256)");

        builder.Property(p => p.ModifiedOn)
            .IsRequired(false)
            .HasColumnName("ModifiedOn")
            .HasColumnType("DATETIME");

        builder.Property(p => p.ModifiedBy)
            .IsRequired(false)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(255)
            .HasColumnType("VARCHAR(256)");

        builder.Ignore(e => e.DomainEvents);
    }
}