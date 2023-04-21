using Domain.Core.Entities;
using Domain.Core.Interfaces;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class SystemGlobalizationRepository : Repository<SystemGlobalization>, ISystemGlobalizationRepository
{
     public SystemGlobalizationRepository(SqlContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<SystemGlobalization>> GetAllAsync()
        => await base.Context.SystemGlobalizations.AsNoTracking().ToListAsync();

    public async Task<SystemGlobalization> GetByKeyAsync(string key)
        => await base.Context.SystemGlobalizations.Where(p => p.Key == key).AsNoTracking().FirstOrDefaultAsync();
}