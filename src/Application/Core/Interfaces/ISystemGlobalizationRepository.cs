using Application.Common.Interfaces;
using Domain.Core.Entities;

namespace Application.Core.Interfaces;

public interface ISystemGlobalizationRepository : IRepository<SystemGlobalization>
{
    Task<IEnumerable<SystemGlobalization>> GetAllAsync();

    Task<SystemGlobalization> GetByKeyAsync(string key);
}