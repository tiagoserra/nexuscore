using Application.Common.Types;
using Domain.Common.Entities;

namespace Application.Common.Interfaces;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task InsertAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task<TEntity> GetByIdAsync(long id);

    Task<ResultPagination> GetByPaginatedAsync(int pageNumber = 1, int pageSize = 25);
}