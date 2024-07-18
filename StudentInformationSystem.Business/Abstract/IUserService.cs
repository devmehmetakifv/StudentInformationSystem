using StudentInformationSystem.Business.Generic.Abstract;
using StudentInformationSystem.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Business.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        public string GetUserEmailAddress(User user);
        public string GetUserRoleByUserName(string userName);
        public IEnumerable<User> GetStudentsByInstructor(int departmendId);
        public IEnumerable<User> GetInstructorsByDepartment(int departmentId);
        public User GetUserByEmail(string email);
        public User GetUserByNames(string firstName, string lastName);
    }
}
