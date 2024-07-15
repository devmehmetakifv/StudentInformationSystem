using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Data.Repositories.Generic.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            return await _context.Set<T>().FindAsync(id) ?? throw new ArgumentNullException();
        }
        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id) ?? throw new ArgumentNullException();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
