using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentInformationSystem.Data.Repositories.Abstract;
using StudentInformationSystem.Data.Repositories.Generic.Concrete;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Data.Repositories.Concrete
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly IRoleRepository _roleRepository;
        public UserRepository(ApplicationDbContext context, IRoleRepository roleRepository) : base(context)
        {
            _roleRepository = roleRepository;
        }
        public string GetUserEmailAddress(User user)
        {
            return user.Email;
        }
        public string GetUserRoleByUserName(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            return _roleRepository.GetById(user.RoleId).Result.Name;
        }
        public User GetStudentByProgramId(int programId)
        {
            return _context.Users.FirstOrDefault(u => u.ProgramID == programId);
        }
        public IEnumerable<User> GetInstructorsByDepartment(int departmentId)
        {
            return _context.Users.Where(u => u.DepartmentID == departmentId).ToList();
        }
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
