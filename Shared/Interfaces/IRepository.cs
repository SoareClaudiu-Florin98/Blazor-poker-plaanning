using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPokerPlanning.Shared.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<TEntity>GetByIdAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task InsertAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
