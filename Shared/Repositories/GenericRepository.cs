using BlazorPokerPlanning.Shared.Context;
using BlazorPokerPlanning.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Shared.Repositories
{
    public abstract class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly DbContext _database;

        public GenericRepository(PokerPlanningDbContext pokerPlanningDbContext)
        {
            _database = pokerPlanningDbContext;
            _database.Database.EnsureCreated();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _database.Set<TEntity>().Remove(entity);
            await _database.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _database.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _database.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            _database.Set<TEntity>().Add(entity);
            await _database.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _database.Set<TEntity>().Update(entity);
            await _database.SaveChangesAsync();
        }
    }
}
