using StudentInformationSystem.Business.Generic.Abstract;
using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Generic.Concrete
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAll();
        }

        public async Task AddAsync(T entity)
        {
            await _repository.Add(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repository.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetById(id);
            if (entity != null)
            {
                await _repository.Delete(entity);
            }
        }
    }
}
