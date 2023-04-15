using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Core.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Data.Mappings;

public class SystemGlobalizationMapping : EntityMapping<SystemGlobalization>
{
    public override void Configure(EntityTypeBuilder<SystemGlobalization> builder)
    {
        builder.Property(p => p.Key).IsRequired().HasColumnName("Key").HasMaxLength(255).HasColumnType("VARCHAR(255)");

        builder.Property(p => p.Resource).IsRequired().HasColumnName("Resource")
            .HasColumnType("NVARCHAR(MAX)")
            .HasConversion(
                v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );

        builder.ToTable("SystemGlobalization");

        base.Configure(builder);
    }
}