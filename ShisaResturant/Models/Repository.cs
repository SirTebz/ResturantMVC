

using Microsoft.EntityFrameworkCore;
using ShisaResturant.Data;

namespace ShisaResturant.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context { get; set; }
        private DbSet<T> _dbSet { get; set; }
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddSync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            T entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllSync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdSync(int id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;
            if (options.HasWhere)
            {
                query = query.Where(options.Where);
            }
            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }
            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }

            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            string primaryKeyName = key.Name;

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
