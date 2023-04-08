using Application.Common.Interfaces;
using Application.Common.Types;
using Domain.Common.Entities;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly SqlContext Context;
    protected readonly DbSet<TEntity> Entity;

    public Repository(SqlContext context)
    {
        Context = context;
        Entity = context.Set<TEntity>();
    }

    public async Task InsertAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");

        Entity.Add(entity);

        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");

        Entity.Update(entity);

        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");

        Entity.Remove(entity);

        await Context.SaveChangesAsync();
    }

    public async Task<TEntity> GetByIdAsync(long id)
        => await Entity.FirstAsync(p => p.Id == id);

    public async Task<ResultPagination> GetByPaginatedAsync(int pageNumber = 1, int pageSize = 25)
    {
        var data = Entity.AsNoTracking().AsQueryable();

        var totalRecords = data.Count();

        var result = await data.OrderBy(p => p.CreatedOn)
            .Skip((pageNumber * pageSize))
            .Take(pageSize)
            .ToListAsync();

        double resultado = totalRecords / pageSize;

        return result is null ?
            new ResultPagination(0, 0, 0, 0, null) :
            new ResultPagination((int)Math.Ceiling(resultado), pageNumber, pageSize, totalRecords, result);
    }
}