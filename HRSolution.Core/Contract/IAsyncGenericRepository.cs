using HRSolution.Infrastructure.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRSolution.Core.Contract
{
    public interface IAsyncGenericRepository<T> where T : BaseObject
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> Query(string where);
        Task<T> DetailsGenerator(T entity, int Id);





    }
}
