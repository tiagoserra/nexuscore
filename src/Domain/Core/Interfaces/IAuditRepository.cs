using Domain.Core.Entities;

namespace Domain.Core.Interfaces;

public interface IAuditRepository
{
    Task InsertAsync(Audit audit);
}