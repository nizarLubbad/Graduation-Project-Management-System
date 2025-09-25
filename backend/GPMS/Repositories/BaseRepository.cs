

using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace GPMS.Repositories
{
    public class BaseRepository<T,TId> : IBaseRepository<T,TId> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
      
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public virtual async Task<T?> GetByIdAsync(TId id) => await _dbSet.FindAsync(id);
        

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity); // Stunet s = new Student({id = 5 ; name = ahmad });

            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

