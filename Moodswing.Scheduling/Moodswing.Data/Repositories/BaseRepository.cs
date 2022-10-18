using Microsoft.EntityFrameworkCore;
using Moodswing.Data.Context;
using Moodswing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moodswing.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;

        private readonly MyContext _context;

        public BaseRepository(MyContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<TEntity> GetAsync(Guid id)
            => await _dbSet.AsNoTracking().SingleOrDefaultAsync(item => item.Id == id);

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity is not null)
            {
                entity.CreateAt = DateTime.UtcNow;
                _dbSet.Add(entity);

                if(await _context.SaveChangesAsync() > default(int))
                {
                    return entity;
                }
            }
            return null;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if(entity is not null)
            {
                entity.UpdateAt = DateTime.UtcNow;
                _dbSet.Update(entity);
                
                if(await _context.SaveChangesAsync() > default(int))
                {
                    return entity;
                }
            }
            return null;
        }
    }
}
