using Domain.Common.Interfaces;
using Domain.Core.Entities;

namespace Domain.Core.Interfaces;

public interface ISystemGlobalizationRepository : IRepository<SystemGlobalization>
{
    Task<IEnumerable<SystemGlobalization>> GetAllAsync();

    Task<SystemGlobalization> GetByKeyAsync(string key);
}