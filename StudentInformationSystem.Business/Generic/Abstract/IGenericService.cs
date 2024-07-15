using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Generic.Abstract
{
    public interface IGenericService<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
        Task DeleteAsync(int id);
    }
}
