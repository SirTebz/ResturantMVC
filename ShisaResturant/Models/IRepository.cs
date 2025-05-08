using System.Linq.Expressions;

namespace ShisaResturant.Models
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllSync();
        Task<T> GetByIdSync(int id, QueryOptions<T> options);
        Task AddSync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}
