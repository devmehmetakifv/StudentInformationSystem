using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentInformationSystem.Data.Repositories.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;

namespace StudentInformationSystem.Data.Repositories.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public string GetUserEmailAddress(User user);
        public string GetUserRoleByUserName(string userName);
        public User GetStudentByProgramId(int programId);
        public IEnumerable<User> GetInstructorsByDepartment(int departmentId);
    }
}
