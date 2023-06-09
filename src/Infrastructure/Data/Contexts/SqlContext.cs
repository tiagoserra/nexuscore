using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Domain.Common.Entities;
using Domain.Core.Entities; 
//%#Domain#%

namespace Infrastructure.Data.Contexts;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> option) : base(option)
    {
    }

    public SqlContext()
    {
    }

    #region DbSets

    public DbSet<SystemGlobalization> SystemGlobalizations { get; set; }  
 	//%#DbSet#%

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(dateTimeConverter);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    
                    entry.Entity.SetCreation(string.Empty);
                    //entry.Entity.Events.Add(new AuditEvent<Entity>(entry.Entity, "insert"));
                    
                    break;

                case EntityState.Modified:

                    entry.Entity.SetModification(string.Empty);
                    //entry.Entity.Events.Add(new AuditEvent<Entity>(entry.Entity, "Update"));

                    break;
                
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    
                    entry.Entity.SetModification(string.Empty);
                    //entry.Entity.Events.Add(new AuditEvent<Entity>(entry.Entity, "Delete"));

                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}