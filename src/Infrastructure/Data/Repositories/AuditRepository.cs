using Domain.Core.Entities;
using Domain.Core.Interfaces;
using Infrastructure.Data.Contexts;

namespace Infrastructure.Data.Repositories;

public class AuditRepository : IAuditRepository
{
    public readonly NoSqlContext _noSqlContext;

    public AuditRepository(NoSqlContext noSqlContext)
        => _noSqlContext = noSqlContext;

    public async Task InsertAsync(Audit audit)
    {
        _noSqlContext.AddCommand(() => _noSqlContext.Audits.InsertOneAsync(audit));
        await _noSqlContext.SaveChanges();
    }
}