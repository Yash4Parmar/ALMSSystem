using Microsoft.EntityFrameworkCore;
//using Alms.DAL.DbModels;
using Alms.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Alms.DAL.Models;
using System.Reflection;

public class DBRepository<TEntity> : IDBRepository<TEntity> where TEntity : class
{
    private readonly WbLearningContext _context;
    private readonly DbSet<TEntity> _entities;

    public DBRepository(WbLearningContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }
    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _entities.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entities.Where(predicate).ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Soft deletes an entity by setting its IsDeleted property to true.
    /// </summary>
    /// <param name="entity">The entity to be soft deleted.</param>
    /// <returns>The task result contains the soft deleted entity.</returns>
    /// <exception cref="ArgumentException"></exception>
    public Task<TEntity> Remove(TEntity entity)
    {
        PropertyInfo? isDeletedProperty = typeof(TEntity).GetProperties()
        .FirstOrDefault(p => p.Name.Equals("IsDeleted", StringComparison.OrdinalIgnoreCase));

        if (isDeletedProperty.CanWrite)
        {
            isDeletedProperty.SetValue(entity, true);
            _entities.Update(entity);
            _context.SaveChangesAsync();
            return Task.FromResult(entity);
        }
        else
        {
            throw new ArgumentException("Entity does not support soft delete.");
        }
    }
}