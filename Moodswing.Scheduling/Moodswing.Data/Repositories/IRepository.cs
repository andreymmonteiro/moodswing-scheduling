using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moodswing.Data.Repositories
{
    public interface IRepository<TEntity> 
    {
        Task<TEntity> GetAsync(Guid id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> DeleteAsync(Guid id);

        Task<TEntity> InsertAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
