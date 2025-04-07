using Ecommorce.Core.interfaces;
using Ecommorce_.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommorce_.infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
           await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {

            var query = _context.Set<T>().AsQueryable();

            foreach (var item in includes)
            {
                query = query.Include(item);
            }
             return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
        }


        public async Task<bool> UpdateAsync(T entity)
        {
            // Get the primary key value dynamically
            var key = _context.Entry(entity).Property("Id").CurrentValue;
            if (key == null)
            {
                return false; // Entity has no valid primary key
            }

            // Check if the entity exists in the database
            var existingEntity = await _context.Set<T>().FindAsync(key);
            if (existingEntity == null)
            {
                return false; // Entity does not exist
            }

            // Mark the entity as modified and save changes
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true; // Update successful
        }


    }
}
